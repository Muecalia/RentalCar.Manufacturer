using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentalCar.Manufacturer.Application.Commands.Request;
using RentalCar.Manufacturer.Application.Queries.Response;
using RentalCar.Manufacturer.Core.Configs;
using RentalCar.Manufacturer.Core.MessageBus;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;

namespace RentalCar.Manufacturer.Application.Services;

public class ManufacturerService : BackgroundService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILoggerService _loggerService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ManufacturerService(IRabbitMqService rabbitMqService, ILoggerService loggerService, IServiceScopeFactory serviceScopeFactory)
    {
        _rabbitMqService = rabbitMqService;
        _loggerService = loggerService;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var scopedFactory = _serviceScopeFactory.CreateScope();
            var scopeService = scopedFactory.ServiceProvider.GetService<IManufacturerRepository>();

            //await GetManufacturer(scopeService, cancellationToken);
            await ValidateNewModelIdManufacturer(scopeService, cancellationToken);
            await ValidateUpdateModelIdManufacturer(scopeService, cancellationToken);

            Console.WriteLine("Manufacturer Service is running");
            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
    
    private async Task ValidateNewModelIdManufacturer(IManufacturerRepository repository, CancellationToken cancellationToken)
    {
        const string title = "Validar código do fornecedor";
        
        using var connection = await _rabbitMqService.CreateConnection(cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        try
        {
            await channel.QueueDeclareAsync(RabbitQueue.MANUFACTURER_MODEL_NEW_REQUEST_QUEUE, true, false, false, null, cancellationToken: cancellationToken);
            await channel.BasicQosAsync(0, 1, false, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonSerializer.Deserialize<RequestValidManufacturer>(message);

                if (request is null)
                {
                    _loggerService.LogWarning(MessageError.NotFound(title));
                    return;
                }
                
                Console.WriteLine($"request -> Id Manufacturer: {request.IdService} - Id Model: {request.IdModel}");
                var manufacturer = await repository.GetById(request.IdService, cancellationToken);
                if (manufacturer is not null)
                {
                    var response = new RequestValidManufacturer(request.IdModel, manufacturer.Id);
                    Console.WriteLine($"response -> Id Manufacturer: {response.IdService} - Id Model: {response.IdModel}");
                    await _rabbitMqService.PublishMessage(response, RabbitQueue.MANUFACTURER_MODEL_NEW_RESPONSE_QUEUE, cancellationToken);
                }
            };

            await channel.BasicConsumeAsync(RabbitQueue.MANUFACTURER_MODEL_NEW_REQUEST_QUEUE, autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(title, ex);
            throw;
        }
        finally
        {
            await _rabbitMqService.CloseConnection(connection, channel, cancellationToken);
        }
    }
    
    private async Task ValidateUpdateModelIdManufacturer(IManufacturerRepository repository, CancellationToken cancellationToken)
    {
        const string title = "Validar código do fornecedor";
        
        using var connection = await _rabbitMqService.CreateConnection(cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        try
        {
            await channel.QueueDeclareAsync(RabbitQueue.MANUFACTURER_MODEL_UPDATE_REQUEST_QUEUE, true, false, false, null, cancellationToken: cancellationToken);
            await channel.BasicQosAsync(0, 1, false, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonSerializer.Deserialize<RequestValidManufacturer>(message);

                if (request is null)
                {
                    _loggerService.LogWarning(MessageError.NotFound(title));
                    return;
                }
                
                Console.WriteLine($"request -> Id Manufacturer: {request.IdService} - Id Model: {request.IdModel}");
                var manufacturer = await repository.GetById(request.IdService, cancellationToken);
                if (manufacturer is not null)
                {
                    var response = new RequestValidManufacturer(request.IdModel, manufacturer.Id);
                    Console.WriteLine($"response -> Id Manufacturer: {response.IdService} - Id Model: {response.IdModel}");
                    await _rabbitMqService.PublishMessage(response, RabbitQueue.MANUFACTURER_MODEL_UPDATE_RESPONSE_QUEUE, cancellationToken);
                }
            };

            await channel.BasicConsumeAsync(RabbitQueue.MANUFACTURER_MODEL_UPDATE_REQUEST_QUEUE, autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(title, ex);
            throw;
        }
        finally
        {
            await _rabbitMqService.CloseConnection(connection, channel, cancellationToken);
        }
    }
    
    private async Task GetManufacturer(IManufacturerRepository repository, CancellationToken cancellationToken)
    {
        const string title = "Pesquisar fornecedor";
        
        using var connection = await _rabbitMqService.CreateConnection(cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        try
        {
            await channel.QueueDeclareAsync(RabbitQueue.MANUFACTURER_MODEL_FIND_REQUEST_QUEUE, true, false, false, null, cancellationToken: cancellationToken);
            await channel.BasicQosAsync(0, 1, false, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var idManufacturer = JsonSerializer.Deserialize<string>(message);

                if (idManufacturer is null)
                {
                    _loggerService.LogWarning(MessageError.NotFound(title));
                    return;
                }
                
                Console.WriteLine($"request -> Id Manufacturer: {idManufacturer}");
                var manufacturer = await repository.GetById(idManufacturer, cancellationToken);
                if (manufacturer is not null)
                {
                    var response = new GetService(manufacturer.Id, manufacturer.Name);
                    Console.WriteLine($"response -> Id Manufacturer: {response.Id} - Id Model: {response.Name}");
                    await _rabbitMqService.PublishMessage(response, RabbitQueue.MANUFACTURER_MODEL_FIND_RESPONSE_QUEUE, cancellationToken);
                }
            };

            await channel.BasicConsumeAsync(RabbitQueue.MANUFACTURER_MODEL_FIND_REQUEST_QUEUE, autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
            await Task.Delay(TimeSpan.FromMicroseconds(2), cancellationToken);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(title, ex);
            throw;
        }
        finally
        {
            await _rabbitMqService.CloseConnection(connection, channel, cancellationToken);
        }
    }
    
}