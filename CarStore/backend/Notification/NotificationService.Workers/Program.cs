using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using N8T.Infrastructure;
using N8T.Infrastructure.Middlewares;
using N8T.Infrastructure.Swagger;
using N8T.Infrastructure.Validator;
using NotificationService.AppCore;
using NotificationService.Services.EmailService;
using NotificationService.Workers;
using SendGrid;
using SendGrid.Extensions.DependencyInjection;

//var builder = WebApplication.CreateBuilder(args);
//var services = builder.Services;
////sendgrid
//builder.Services.AddSendGrid(options =>
//{
//    options.ApiKey = builder.Configuration
//    .GetSection("SendGridEmailSettings").GetValue<string>("APIKey");
//});

//builder.Services.AddTransient<IEmailService, EmailService>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
