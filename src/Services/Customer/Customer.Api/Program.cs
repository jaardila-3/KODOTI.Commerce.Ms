using Customer.Persistence.Database;
using Customer.Service.Queries;
using Customer.Service.Queries.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//IoC Context
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSqlServer"),
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Customer"));
});
//cargamos todo el assembly donde est�n los command para que el mediador los identifique autom�ticamente
builder.Services.AddMediatR(Assembly.Load("Customer.Service.EventHandlers"));
//IoC
builder.Services.AddTransient<IClientQueryService, ClientQueryService>();

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
