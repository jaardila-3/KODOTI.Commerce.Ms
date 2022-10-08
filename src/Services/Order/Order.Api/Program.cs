using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Persistence.Database;
using Order.Service.Proxies;
using Order.Service.Proxies.Contracts;
using Order.Service.Proxies.MsCatalog;
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

// ApiUrls
builder.Services.Configure<ApiUrls>(opts => builder.Configuration.GetSection("ApiUrls").Bind(opts));

// Proxies
builder.Services.AddHttpClient<ICatalogProxy, CatalogProxy>();

// Event handlers
//cargamos todo el assembly donde est�n los command para que el mediador los identifique autom�ticamente
builder.Services.AddMediatR(Assembly.Load("Order.Service.EventHandlers"));

// Query services
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
