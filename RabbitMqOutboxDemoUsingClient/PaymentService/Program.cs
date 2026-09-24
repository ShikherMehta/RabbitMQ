using Microsoft.EntityFrameworkCore;
using OrderService.Workers;
using PaymentService.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<PaymentService.Data.AppDbContext>(options =>
    options.UseSqlServer(
        "Server=Shikher\\SQLEXPRESS;" +
        "Database=OrderPaymentDb;" +
        "Trusted_Connection=True;" +
        "TrustServerCertificate=True;"));
builder.Services.AddSingleton<PaymentService.RabbitMq.RabbitMqConnection>();
builder.Services.AddHostedService<PaymentConsumerWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
