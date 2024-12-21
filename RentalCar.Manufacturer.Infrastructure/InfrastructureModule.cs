using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;
using RentalCar.Manufacturer.Infrastructure.MessageBus;
using RentalCar.Manufacturer.Infrastructure.Prometheus;
using RentalCar.Manufacturer.Infrastructure.Repositories;
using RentalCar.Manufacturer.Infrastructure.Services;

namespace RentalCar.Manufacturer.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) 
    {
        services
            .AddServices()
            .AddOpenTelemetryConfig();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services) 
    {
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
        services.AddSingleton<IPrometheusService, PrometheusService>();

        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();

        return services;
    }
    
    private static IServiceCollection AddOpenTelemetryConfig(this IServiceCollection services)
    {
        const string serviceName = "RentalCar Manufacturer";
        const string serviceVersion = "v1";
        
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion:serviceVersion))
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter()
                .AddConsoleExporter())
            .WithMetrics(metrics => metrics
                .AddConsoleExporter()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter()
            );

        return services;
    }

}