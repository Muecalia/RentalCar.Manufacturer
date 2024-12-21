using MediatR;
using Microsoft.AspNetCore.Authorization;
using RentalCar.Manufacturer.Application.Commands.Request;
using RentalCar.Manufacturer.Application.Queries.Request;

namespace RentalCar.Manufacturer.API.Endpoints;

public static class ManufacturerEndPoint
{
    public static void MapManufacturerEndpoints(this IEndpointRouteBuilder route)
    {
        // Get All Manufacturers
        route.MapGet("/manufacturer", [Authorize(Roles = "Admin")] async (IMediator mediator, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10) => 
        {
            var result = await mediator.Send(new FindAllManufacturersRequest(pageNumber, pageSize), cancellationToken);
            return Results.Ok(result);
        });

        //Get Manufacturer by Id
        route.MapGet("/manufacturer/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new FindManufacturerByIdRequest(id), cancellationToken);
            return result.Succeeded ? Results.Ok(result) : Results.NotFound(result.Message);
        });

        // Create new Manufacturer
        route.MapPost("/manufacturer", [Authorize(Roles = "Admin")] async (CreateManufacturerRequest request, IMediator mediator, CancellationToken cancellationToken) => 
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Succeeded ? Results.Created("", result) : Results.BadRequest(result.Message);
        });

        // Delete Manufacturer
        route.MapDelete("/manufacturer/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, CancellationToken cancellationToken) => 
        {
            var result = await mediator.Send(new DeleteManufacturerRequest(id), cancellationToken);
            return result.Succeeded ? Results.Ok(result) : Results.NotFound(result.Message);
        });

        // Update Manufacturer
        route.MapPut("/manufacturer/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, UpdateManufacturerRequest request, CancellationToken cancellationToken) =>
        {
            request.Id = id;
            var result = await mediator.Send(request, cancellationToken);
            return result.Succeeded ? Results.Created("", result) : Results.BadRequest(result.Message);
        });
    }
}
