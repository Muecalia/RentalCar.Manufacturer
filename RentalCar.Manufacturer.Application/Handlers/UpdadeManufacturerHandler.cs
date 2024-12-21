﻿using MediatR;
using RentalCar.Manufacturer.Application.Commands.Request;
using RentalCar.Manufacturer.Application.Commands.Response;
using RentalCar.Manufacturer.Core.Configs;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;
using RentalCar.Manufacturer.Core.Wrappers;

namespace RentalCar.Manufacturer.Application.Handlers;

public class UpdadeManufacturerHandler : IRequestHandler<UpdateManufacturerRequest, ApiResponse<InputManufacturerResponse>>
    {
        private readonly IManufacturerRepository _repository;
        private readonly ILoggerService _loggerService;

        public UpdadeManufacturerHandler(IManufacturerRepository repository, ILoggerService loggerService)
        {
            _repository = repository;
            _loggerService = loggerService;
        }

        public async Task<ApiResponse<InputManufacturerResponse>> Handle(UpdateManufacturerRequest request, CancellationToken cancellationToken)
        {
            const string Objecto = "fabricante";
            const string Operacao = "editar";
            try
            {
                var manufacturer = await _repository.GetById(request.Id, cancellationToken);
                if (manufacturer == null)
                {
                    _loggerService.LogWarning(MessageError.NotFound(Objecto, request.Id));
                    return ApiResponse<InputManufacturerResponse>.Error(MessageError.NotFound(Objecto));
                }

                manufacturer.Name = request.Name;
                manufacturer.Phone = request.Phone;
                manufacturer.Email = request.Email;

                await _repository.Update(manufacturer, cancellationToken);

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
