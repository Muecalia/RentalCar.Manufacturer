using MediatR;
using Microsoft.AspNetCore.Http;
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
    private readonly IPrometheusService _prometheusService;

    public FindManufacturerByIdHandler(IManufacturerRepository manufacturerRepository, ILoggerService loggerService, IPrometheusService prometheusService)
    {
        _manufacturerRepository = manufacturerRepository;
        _loggerService = loggerService;
        _prometheusService = prometheusService;
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
                _prometheusService.AddFindByIdManufacturerCounter(StatusCodes.Status404NotFound.ToString());
                return ApiResponse<FindManufacturerResponse>.Error(MessageError.NotFound(Objecto));
            }

            var result = new FindManufacturerResponse(manufacturer.Id, manufacturer.Name, manufacturer.Phone, manufacturer.Email ?? string.Empty);
            _prometheusService.AddFindByIdManufacturerCounter(StatusCodes.Status200OK.ToString());
            _loggerService.LogInformation(MessageError.CarregamentoSucesso(Objecto, 1));
            return ApiResponse<FindManufacturerResponse>.Success(result, MessageError.CarregamentoSucesso(Objecto));
        }
        catch (Exception ex)
        {
            _prometheusService.AddFindByIdManufacturerCounter(StatusCodes.Status400BadRequest.ToString());
            _loggerService.LogError(MessageError.CarregamentoErro(Objecto, ex.Message));
            return ApiResponse<FindManufacturerResponse>.Error(MessageError.CarregamentoErro(Objecto));
        }
    }
}

