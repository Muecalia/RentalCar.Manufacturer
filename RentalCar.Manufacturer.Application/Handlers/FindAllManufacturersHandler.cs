using MediatR;
using Microsoft.AspNetCore.Http;
using RentalCar.Manufacturer.Application.Queries.Request;
using RentalCar.Manufacturer.Application.Queries.Response;
using RentalCar.Manufacturer.Core.Configs;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;
using RentalCar.Manufacturer.Core.Wrappers;

namespace RentalCar.Manufacturer.Application.Handlers;

public class FindAllManufacturersHandler : IRequestHandler<FindAllManufacturersRequest, PagedResponse<FindManufacturerResponse>>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly ILoggerService _loggerService;
    private readonly IPrometheusService _prometheusService;

    public FindAllManufacturersHandler(IManufacturerRepository manufacturerRepository, ILoggerService loggerService, IPrometheusService prometheusService)
    {
        _manufacturerRepository = manufacturerRepository;
        _loggerService = loggerService;
        _prometheusService = prometheusService;
    }

    public async Task<PagedResponse<FindManufacturerResponse>> Handle(FindAllManufacturersRequest request, CancellationToken cancellationToken)
    {
        const string Objecto = "fabricantes";
        try
        {
            var manufacturers = await _manufacturerRepository.GetAll(request.PageNumber, request.PageSize, cancellationToken);
            _prometheusService.AddFindAllManufacturersCounter(StatusCodes.Status200OK.ToString());
            var results = manufacturers.Select(manufacturer => new FindManufacturerResponse(manufacturer.Id, manufacturer.Name, manufacturer.Phone, manufacturer.Email)).ToList();
            return new PagedResponse<FindManufacturerResponse>(results, request.PageNumber, request.PageSize, results.Count, MessageError.CarregamentoSucesso(Objecto, results.Count));
        }
        catch (Exception ex)
        {
            _prometheusService.AddFindAllManufacturersCounter(StatusCodes.Status500InternalServerError.ToString());
            _loggerService.LogError(MessageError.CarregamentoErro(Objecto, ex.Message));
            return new PagedResponse<FindManufacturerResponse>(MessageError.CarregamentoErro(Objecto));
            //throw;
        }
    }

}
