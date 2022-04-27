using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.BusinessLogic.Models;
using Authentication.BusinessLogic.Repositories;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Shared.RabbitMQ
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

        public Receiver(IOptions<RabbitMqOptions> rabbitMqOptions, IServiceScopeFactory scopeFactory)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            InitializeRabbitMqListener();
            this.scopeFactory = scopeFactory;
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
            _channel.QueueDeclare(queue: "User." + _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var updateStatusModel = JsonConvert.DeserializeObject<AuthUpdateStatusModel>(content);

                HandleMessage(updateStatusModel);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("User." + _queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async void HandleMessage(AuthUpdateStatusModel updateCustomerFullNameModel)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var UserService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                var auth = await UserService.GetByUserId(updateCustomerFullNameModel.UserId);
                auth.Status = Status.Approved;
                await UserService.Update(auth);
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