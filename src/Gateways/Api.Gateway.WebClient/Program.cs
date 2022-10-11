using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// ApiUrls
//llena la clase con los datos que están en el appsettings.json para pasarla por ioc
builder.Services.Configure<ApiUrls>(opts => builder.Configuration.GetSection("ApiUrls").Bind(opts));

// Proxies
//inyecta el HttpClient por IoC
builder.Services.AddHttpClient<IOrderProxy, OrderProxy>();
builder.Services.AddHttpClient<ICustomerProxy, CustomerProxy>();
builder.Services.AddHttpClient<ICatalogProxy, CatalogProxy>();

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
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Api.Gateway.WebClient", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Encabezado de autorización JWT utilizando el esquema Bearer."

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
app.UseAuthentication();

app.MapControllers();

app.Run();
