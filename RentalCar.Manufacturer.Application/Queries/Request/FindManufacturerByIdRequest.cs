using MediatR;
using RentalCar.Manufacturer.Core.Wrappers;
using RentalCar.Manufacturer.Application.Queries.Response;

namespace RentalCar.Manufacturer.Application.Queries.Request
{
    public class FindManufacturerByIdRequest(string id) : IRequest<ApiResponse<FindManufacturerResponse>>
    {
        public string Id { get; set; } = id;
    }
}
