using MediatR;
using RentalCar.Manufacturer.Application.Queries.Response;
using RentalCar.Manufacturer.Core.Wrappers;

namespace RentalCar.Manufacturer.Application.Queries.Request;

public class FindAllManufacturersRequest(int pageNumber, int pageSize) : IRequest<PagedResponse<FindManufacturerResponse>>
{
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
}

