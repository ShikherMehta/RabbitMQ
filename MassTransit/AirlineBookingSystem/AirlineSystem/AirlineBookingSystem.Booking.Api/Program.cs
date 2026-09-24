using System.Data;
using System.Reflection;
using AirlineBookingSystem.Booking.Api.Consumers;
using AirlineBookingSystem.Booking.Application.Commands;
using AirlineBookingSystem.Booking.Application.Handlers;
using AirlineBookingSystem.Booking.Core.Repositories;
using AirlineSystem.Booking.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped <IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);
var assemblies = new Assembly[]
    {
    Assembly.GetExecutingAssembly(),
    typeof(CreateBookingHandler).Assembly,
    typeof(GetBookingHandler).Assembly
    };
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<NotificationEventConsumer>();
    config.UsingRabbitMq((ct, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.NotificationSentQueue, c =>
        {
            c.ConfigureConsumer<NotificationEventConsumer>(ct);
        });
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
