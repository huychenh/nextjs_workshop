using Confluent.Kafka;
using KafkaConsumer;
using System.Text.Json;
using NotificationService.AppCore.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using NotificationService.Services.EmailService;
using SendGrid.Helpers.Mail;
using SendGrid;
using NotificationService.Models;
using Microsoft.Extensions.DependencyInjection;


namespace NotificationService.Workers
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        //private readonly ISendGridClient _sendGridClient;
        //private readonly IEmailService _emailService;
        IServiceProvider _serviceProvider;
        protected readonly string _fromEmail;
        protected readonly string _fromName;
        //ISendGridClient _sendGridClient;
        public Worker(
            ILogger<Worker> logger,
           // IEmailService emailService,
            //ISendGridClient sendGridClient,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _logger = logger;
          // _emailService = emailService;

            //_sendGridClient = sendGridClient;
            _serviceProvider = serviceProvider;
            _fromEmail = configuration.GetSection("SendGridEmailSettings")
                                     .GetValue<string>("FromEmail");
            _fromName = configuration.GetSection("SendGridEmailSettings")
                                      .GetValue<string>("FromName");
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await DoWorkSendMail(stoppingToken);

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task DoWorkSendMail(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

                var config = new ConsumerConfig
                {
                    BootstrapServers = "localhost:29092",
                    GroupId = "normal",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(new[] { "order" });

                    var consumeResult = consumer.Consume(stoppingToken);

                    if (consumeResult.Message != null)
                    {
                        Console.WriteLine("Received: " + consumeResult.Message.Value);

                        OrderCreatedEvent orderCreatedModel = JsonSerializer.Deserialize<OrderCreatedEvent>(consumeResult.Message.Value);

                        var request = new SendNotification.Command();
                        var cancellationToken = new CancellationToken();

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            await scope.ServiceProvider.GetRequiredService<IEmailService>().SendEmail(new Models.EmailDto
                            {
                                FromEmail = _fromEmail,
                                FromName = _fromName,
                                Subject = orderCreatedModel.ProductName,
                                Body = $"Thanks for ordered product{orderCreatedModel.ProductName} with code {orderCreatedModel.OrderId}",
                                To = orderCreatedModel.BuyerEmail
                            });
                        }
                    }
                    // todo: properly close consumer
                    consumer.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}