﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RentalCar.Manufacturer.Application.Handlers;
using RentalCar.Manufacturer.Application.Services;
using RentalCar.Manufacturer.Application.Validators;

namespace RentalCar.Manufacturer.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddFluentValidation()
            .AddHandlers()
            .AddBackgroundService();
        return services;
    }


    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<CreateManufacturerValidator>();

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<CreateManufacturerHandler>());

        return services;
    }
    
    private static IServiceCollection AddBackgroundService(this IServiceCollection services)
    {
        services.AddHostedService<ManufacturerService>();
        services.AddHostedService<FindManufacturerService>();
        return services;
    }

}

