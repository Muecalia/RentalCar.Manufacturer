using MediatR;
using RentalCar.Manufacturer.Application.Commands.Request;
using RentalCar.Manufacturer.Application.Commands.Response;
using RentalCar.Manufacturer.Core.Configs;
using RentalCar.Manufacturer.Core.Entities;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;
using RentalCar.Manufacturer.Core.Wrappers;

namespace RentalCar.Manufacturer.Application.Handlers;

public class CreateManufacturerHandler : IRequestHandler<CreateManufacturerRequest, ApiResponse<InputManufacturerResponse>>
{
    private readonly IManufacturerRepository _repository;
    private readonly ILoggerService _loggerService;

    public CreateManufacturerHandler(IManufacturerRepository repository, ILoggerService loggerService)
    {
        _repository = repository;
        _loggerService = loggerService;
    }

    public async Task<ApiResponse<InputManufacturerResponse>> Handle(CreateManufacturerRequest request, CancellationToken cancellationToken)
    {
        const string Objecto = "fabricante";
        const string Operacao = "registar";
        try
        {
            if (await _repository.IsManufacturerExist(request.Name, cancellationToken))
            {
                _loggerService.LogWarning(MessageError.Conflito($"{Objecto} {request.Name}"));
                return ApiResponse<InputManufacturerResponse>.Error(MessageError.Conflito(Objecto));
            }

            var newManufacturer = new Manufacturers
            {
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email
            };

            var manufacturer = await _repository.Create(newManufacturer, cancellationToken);

            var result = new InputManufacturerResponse(manufacturer.Id, manufacturer.Name);

            return ApiResponse<InputManufacturerResponse>.Success(result, MessageError.OperacaoSucesso(Objecto, Operacao));
        }
        catch (Exception ex)
        {
            _loggerService.LogError(MessageError.OperacaoErro(Objecto, Operacao, ex.Message));
            return ApiResponse<InputManufacturerResponse>.Error(MessageError.OperacaoErro(Objecto, Operacao));
        }
    }
}
