using AirlineBookingSystem.Flights.Application.Handlers;
using AirlineBookingSystem.Flights.Application.Queries;
using AirlineBookingSystem.Flights.Infratsructure.Repositories;
using AIrlineBookingSystem.Flights.Core.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using MassTransit;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);
var assemblies = new Assembly[]
    {
    Assembly.GetExecutingAssembly(),
    typeof(CreateFlightHandler).Assembly,
    typeof(DeleteFlightHandler).Assembly,
    typeof(GetAllFlightHandler).Assembly
    };

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
