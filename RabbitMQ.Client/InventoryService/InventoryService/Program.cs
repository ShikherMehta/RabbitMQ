using InventoryService;
using InventoryService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(
        "Server=Shikher\\SQLEXPRESS;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();