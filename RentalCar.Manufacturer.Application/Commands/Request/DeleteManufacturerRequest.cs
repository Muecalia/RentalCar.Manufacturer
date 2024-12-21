using MediatR;
using RentalCar.Manufacturer.Application.Commands.Response;
using RentalCar.Manufacturer.Core.Wrappers;

namespace RentalCar.Manufacturer.Application.Commands.Request;
public class DeleteManufacturerRequest(string id) : IRequest<ApiResponse<InputManufacturerResponse>>
{
    public string Id { get; set; } = id;
}

