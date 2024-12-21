using MediatR;
using Microsoft.AspNetCore.Http;
using RentalCar.Manufacturer.Application.Commands.Request;
using RentalCar.Manufacturer.Application.Commands.Response;
using RentalCar.Manufacturer.Core.Configs;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;
using RentalCar.Manufacturer.Core.Wrappers;

namespace RentalCar.Manufacturer.Application.Handlers;

public class DeleteManufacturerHandler : IRequestHandler<DeleteManufacturerRequest, ApiResponse<InputManufacturerResponse>>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly ILoggerService _loggerService;
    private readonly IPrometheusService _prometheusService;

    public DeleteManufacturerHandler(IManufacturerRepository manufacturerRepository, ILoggerService loggerService, IPrometheusService prometheusService)
    {
        _manufacturerRepository = manufacturerRepository;
        _loggerService = loggerService;
        _prometheusService = prometheusService;
    }

    public async Task<ApiResponse<InputManufacturerResponse>> Handle(DeleteManufacturerRequest request, CancellationToken cancellationToken)
    {
        const string Objecto = "fabricante";
        const string Operacao = "eliminar";
        try
        {
            var manufacturer = await _manufacturerRepository.GetById(request.Id, cancellationToken);
            if (manufacturer == null)
            {
                _loggerService.LogWarning(MessageError.NotFound(Objecto, request.Id));
                _prometheusService.AddDeleteManufacturerCounter(StatusCodes.Status404NotFound.ToString());
                return ApiResponse<InputManufacturerResponse>.Error(MessageError.NotFound(Objecto));
            }

            await _manufacturerRepository.Delete(manufacturer, cancellationToken);

            var result = new InputManufacturerResponse(manufacturer.Id, manufacturer.Name);
            _prometheusService.AddDeleteManufacturerCounter(StatusCodes.Status204NoContent.ToString());
            return ApiResponse<InputManufacturerResponse>.Success(result, MessageError.OperacaoSucesso(Objecto, Operacao));
        }
        catch (Exception ex)
        {
            _prometheusService.AddDeleteManufacturerCounter(StatusCodes.Status400BadRequest.ToString());
            _loggerService.LogError(MessageError.OperacaoErro(Objecto, Operacao, ex.Message));
            return ApiResponse<InputManufacturerResponse>.Error(MessageError.OperacaoErro(Objecto, Operacao));
            //throw;
        }
    }
}

