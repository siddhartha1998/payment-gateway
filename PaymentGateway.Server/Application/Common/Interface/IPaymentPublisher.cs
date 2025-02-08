using Newtonsoft.Json;
using PaymentGateway.Server.Application.Payments.Commands;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace PaymentGateway.Server.Application.Common.Interface
{
    public interface IPaymentPublisher
    {
        Task<string> PublishPaymentRequest(PaymentProcessRequest request);
    }

    public class PaymentPublisherService : IPaymentPublisher
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _requestQueueName;
        private readonly string _responseQueueName;
        private readonly string _queueName = "paymentQueue";

        public PaymentPublisherService(IConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                Port = int.Parse(_configuration["RabbitMQ:PortNo"]),
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _requestQueueName = _configuration["RabbitMQ:RequestQueueName"];
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }


        public async Task<string> PublishPaymentRequest(PaymentProcessRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var replyQueueName = _channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(_channel);

            var tcs = new TaskCompletionSource<string>();

            consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var response = Encoding.UTF8.GetString(ea.Body.ToArray());
                    tcs.SetResult(response);
                }
            };

            _channel.BasicConsume(queue: replyQueueName, autoAck: true, consumer: consumer);

            var properties = _channel.CreateBasicProperties();
            properties.ReplyTo = replyQueueName;
            properties.CorrelationId = correlationId;

            var message = JsonConvert.SerializeObject(request);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: properties, body: body);

            return await tcs.Task;
        }
    }
}
