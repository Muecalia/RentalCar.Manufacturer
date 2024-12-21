using MediatR;
using RentalCar.Manufacturer.Application.Queries.Request;
using RentalCar.Manufacturer.Application.Queries.Response;
using RentalCar.Manufacturer.Core.Configs;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;
using RentalCar.Manufacturer.Core.Wrappers;

namespace RentalCar.Manufacturer.Application.Handlers;

public class FindManufacturerByIdHandler : IRequestHandler<FindManufacturerByIdRequest, ApiResponse<FindManufacturerResponse>>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly ILoggerService _loggerService;

    public FindManufacturerByIdHandler(IManufacturerRepository manufacturerRepository, ILoggerService loggerService)
    {
        _manufacturerRepository = manufacturerRepository;
        _loggerService = loggerService;
    }

    public async Task<ApiResponse<FindManufacturerResponse>> Handle(FindManufacturerByIdRequest request, CancellationToken cancellationToken)
    {
        const string Objecto = "fabricante";
        try
        {
            var manufacturer = await _manufacturerRepository.GetById(request.Id, cancellationToken);
            if (manufacturer == null)
            {
                _loggerService.LogWarning(MessageError.NotFound(Objecto, request.Id));
                return ApiResponse<FindManufacturerResponse>.Error(MessageError.NotFound(Objecto));
            }

            var result = new FindManufacturerResponse(manufacturer.Id, manufacturer.Name, manufacturer.Phone, manufacturer.Email ?? string.Empty);

            _loggerService.LogInformation(MessageError.CarregamentoSucesso(Objecto, 1));
            return ApiResponse<FindManufacturerResponse>.Success(result, MessageError.CarregamentoSucesso(Objecto));
        }
        catch (Exception ex)
        {
            _loggerService.LogError(MessageError.CarregamentoErro(Objecto, ex.Message));
            return ApiResponse<FindManufacturerResponse>.Error(MessageError.CarregamentoErro(Objecto));
        }
    }
}

