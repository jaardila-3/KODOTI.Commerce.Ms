using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Persistence.Database;
using Order.Service.Queries;
using Order.Service.Queries.Contracts;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//IoC Context
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSqlServer"),
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Order"));
});
//cargamos todo el assembly donde están los command para que el mediador los identifique automáticamente
builder.Services.AddMediatR(Assembly.Load("Order.Service.EventHandlers"));
//IoC
builder.Services.AddTransient<IOrderQueryService, OrderQueryService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
