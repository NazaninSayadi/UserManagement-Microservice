﻿using System.Text;
using Authentication.BusinessLogic.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Shared.RabbitMQ
{
    public class AuthenticationAddedSender : IAuthenticationAddedSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;

        public AuthenticationAddedSender(IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _queueName = rabbitMqOptions.Value.QueueName;
            _hostname = rabbitMqOptions.Value.Hostname;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            CreateConnection();
        }

        public void SendAuthentication(Guid userId, string firstName, string lastName, string address, string education)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "Auth." + _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var user = new UserCreateModel { UserId = userId, FirstName = firstName, LastName = lastName, Address = address, Education = education };
                    var json = JsonConvert.SerializeObject(user);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: "Auth." + _queueName, basicProperties: null, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }

    }
}