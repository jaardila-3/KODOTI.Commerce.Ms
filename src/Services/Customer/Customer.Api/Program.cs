using Customer.Persistence.Database;
using Customer.Service.Queries;
using Customer.Service.Queries.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

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

// Add Authentication
var secretKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("SecretKey"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//configurar swagger para que reciba token Bearer
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer.Api", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Encabezado de autorizaci�n JWT utilizando el esquema Bearer."

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                {
                                    {
                                          new OpenApiSecurityScheme
                                          {
                                              Reference = new OpenApiReference
                                              {
                                                  Type = ReferenceType.SecurityScheme,
                                                  Id = "Bearer"
                                              }
                                          },
                                         new List<string>()
                                    }
                                });
});

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
