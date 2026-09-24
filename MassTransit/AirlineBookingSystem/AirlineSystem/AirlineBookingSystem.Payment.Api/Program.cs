using AirlineBookingSystem.Payment.Application.Handler;
using AirlineBookingSystem.Payment.Core.Repositories;
using AirlineBookingSystem.Payment.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using MassTransit;
using AirlineBookingSystem.Payment.Application.Consumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);
    var assemblies = new Assembly[]
    {
        Assembly.GetExecutingAssembly(),
        typeof(ProcessPaymentHandler).Assembly,
        typeof(RefundPaymentHandler).Assembly
    };

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<FlightBookedConsumer>();
    config.UsingRabbitMq((ct, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.FlightBookedQueue, c =>
        {
            c.ConfigureConsumer<FlightBookedConsumer>(ct);
        });
    });
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
