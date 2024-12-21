using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RentalCar.Manufacturer.API.Endpoints;
using RentalCar.Manufacturer.Application;
using RentalCar.Manufacturer.Core.Configs;
using RentalCar.Manufacturer.Core.MessageBus;
using RentalCar.Manufacturer.Infrastructure;
using RentalCar.Manufacturer.Infrastructure.Persistence;
using Serilog;


const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

//LOG
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/rentalcarmanufacturer.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connection String
builder.Services.AddDbContextPool<ManufacturerContext>(opt =>
    opt.UseMySql(builder.Configuration.GetConnectionString("ManufacturerConnetion"), new MySqlServerVersion(new Version(8, 0, 40)))
);

//InfrastructureModule
builder.Services.AddInfrastructure();

//ApplicationModule
builder.Services.AddApplication();

//Ler as configurações do RabbitMq
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMqConfig"));

//Ler as configurações do JWT
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RentalCar Manufacturer", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o esquema Bearer."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
         }
     });
});

builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,

          ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
          ValidAudience = builder.Configuration["JwtConfig:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:SecretKey"]))
      };
  });

builder.Services.AddCors(options =>
{
    /*options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://example.com", "http://www.contoso.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });*/
    options.AddPolicy(myAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//Autorização
builder.Services.AddAuthorization();
/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});*/

var app = builder.Build();

//Add Endpoints
app.MapGroup("api")
    .WithTags("manufacturer")
    .MapManufacturerEndpoints();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentalCar Manufacturer v1"));
}

//OpenTelemetry to Prometheus
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseCors(myAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
