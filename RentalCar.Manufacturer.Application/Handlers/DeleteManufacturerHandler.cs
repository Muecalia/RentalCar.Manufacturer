using MediatR;
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

    public DeleteManufacturerHandler(IManufacturerRepository manufacturerRepository, ILoggerService loggerService)
    {
        _manufacturerRepository = manufacturerRepository;
        _loggerService = loggerService;
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
                return ApiResponse<InputManufacturerResponse>.Error(MessageError.NotFound(Objecto));
            }

            await _manufacturerRepository.Delete(manufacturer, cancellationToken);

            var result = new InputManufacturerResponse(manufacturer.Id, manufacturer.Name);

            return ApiResponse<InputManufacturerResponse>.Success(result, MessageError.OperacaoSucesso(Objecto, Operacao));
        }
        catch (Exception ex)
        {
            _loggerService.LogError(MessageError.OperacaoErro(Objecto, Operacao, ex.Message));
            return ApiResponse<InputManufacturerResponse>.Error(MessageError.OperacaoErro(Objecto, Operacao));
            //throw;
        }
    }
}

