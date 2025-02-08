using Newtonsoft.Json;
using PaymentGateway.Server.Application.Payments.Commands;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PaymentGateway.Server.Application.Common.Services
{

    public class PaymentConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ConnectionFactory _factory;
        private readonly string _responseQueueName;
        private readonly string _queueName = "paymentQueue";
        public PaymentConsumerService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                Port = int.Parse(_configuration["RabbitMQ:PortNo"]),
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"],
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
            _factory.AutomaticRecoveryEnabled = true;
            _responseQueueName = _configuration["RabbitMQ:ResponseQueueName"];
        }
        public void ConsumePaymentResponseAsync()
        {


            //var tcs = new TaskCompletionSource<PaymentResponse>();

            //lock (_pendingResponses)
            //{
            //    _pendingResponses[correlationId] = tcs;
            //}

            //using var connection = _factory.CreateConnection();
            //using var channel = connection.CreateModel();

            //channel.QueueDeclare(queue: _responseQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            //var consumer = new EventingBasicConsumer(channel);
            //consumer.Received += (model, ea) =>
            //{
            //    var body = ea.Body.ToArray();
            //    var message = Encoding.UTF8.GetString(body);
            //    var response = JsonConvert.DeserializeObject<PaymentResponse>(message);

            //    if (response != null && _pendingResponses.TryGetValue(correlationId, out var responseTcs))
            //    {
            //        responseTcs.SetResult(response);
            //        lock (_pendingResponses)
            //        {
            //            _pendingResponses.Remove(correlationId);
            //        }
            //    }
            //};

            //channel.BasicConsume(queue: _responseQueueName, autoAck: true, consumer: consumer);

            //return await tcs.Task.WaitAsync(TimeSpan.FromSeconds(15));
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var paymentRequest = JsonConvert.DeserializeObject<PaymentProcessRequest>(message);

                Console.WriteLine($"[x] Received Payment Request: {paymentRequest.InvoiceNo}");

                var response = await CallThirdPartyApi(paymentRequest);

                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

                var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

                channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: replyProps, body: responseBytes);
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public async Task<PaymentProcessResponse> CallThirdPartyApi(PaymentProcessRequest request)
        {
            string url = _configuration["DemoPaymentAPI:APIUrl"];
            string req = JsonConvert.SerializeObject(request);
            var jsonContent = new StringContent(req, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    return new PaymentProcessResponse
                    {
                        InvoiceNo = request.InvoiceNo,
                        Message = error,
                        ReferenceNo = 0,
                        Status = Domain.Enums.TransactionStatus.Failed,
                        TransactionDateTime = DateTime.UtcNow,
                    };
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var paymentResponse = JsonConvert.DeserializeObject<PaymentProcessResponse>(responseString);
                if (paymentResponse == null)
                {
                    return new PaymentProcessResponse
                    {
                        InvoiceNo = request.InvoiceNo,
                        Message = "Failed to parse response",
                        ReferenceNo = 0,
                        Status = Domain.Enums.TransactionStatus.Failed,
                        TransactionDateTime = DateTime.UtcNow,
                    };
                }
                return paymentResponse;
            }
            catch (HttpRequestException ex)
            {
                return new PaymentProcessResponse
                {
                    InvoiceNo = request.InvoiceNo,
                    Message = $"Could not connect to the server: {url}",
                    ReferenceNo = 0,
                    Status = Domain.Enums.TransactionStatus.Failed,
                    TransactionDateTime = DateTime.UtcNow,
                };
            }
            catch (Exception ex)
            {
                return new PaymentProcessResponse
                {
                    InvoiceNo = request.InvoiceNo,
                    Message = $"Error while calling the server",
                    ReferenceNo = 0,
                    Status = Domain.Enums.TransactionStatus.Failed,
                    TransactionDateTime = DateTime.UtcNow,
                };
            }

        }

    }
}
