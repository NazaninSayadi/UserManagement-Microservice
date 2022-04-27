using System.Text;
using User.BusinessLogic.Models;
using User.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ
{
    public class Receiver : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IServiceScopeFactory scopeFactory;

        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly IUserAddedSender _userAddedSender;


        public Receiver(IOptions<RabbitMqOptions> rabbitMqOptions, IServiceScopeFactory scopeFactory, IUserAddedSender userAddedSender)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            InitializeRabbitMqListener();
            this.scopeFactory = scopeFactory;
            _userAddedSender = userAddedSender;
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "Auth." + _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var userCreateModel = JsonConvert.DeserializeObject<UserCreateModel>(content);

                HandleMessage(userCreateModel);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("Auth." + _queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async void HandleMessage(UserCreateModel userCreateModel)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var UserService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var user = await UserService.GetByUserId(userCreateModel.UserId);
                if (user != null)
                    throw new Exception("User is repetitive");

                await UserService.Register(userCreateModel.UserId, userCreateModel.FirstName, userCreateModel.LastName, userCreateModel.Address, userCreateModel.Education);
                _userAddedSender.SendUser(userCreateModel.UserId);
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}