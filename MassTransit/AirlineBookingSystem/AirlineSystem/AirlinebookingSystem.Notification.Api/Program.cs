using AirlineBookingSystem.Notification.Application.Consumer;
using AirlineBookingSystem.Notification.Application.Handlers;
using AirlineBookingSystem.Notification.Application.Interfaces;
using AirlineBookingSystem.Notification.Application.Services;
using AirlineBookingSystem.Notification.Core.Repositories;
using AirlineBookingSystem.Notification.Infrastructure.Repository;
using MassTransit;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);
var assemblies = new Assembly[]
{
        Assembly.GetExecutingAssembly(),
        typeof(SendNotificationHandler).Assembly
};
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(assemblies);
});
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<PaymentProcessedConsumer>();
    config.UsingRabbitMq((ct, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.PaymentProcessedQueue, c =>
        {
            c.ConfigureConsumer<PaymentProcessedConsumer>(ct);
        });
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
