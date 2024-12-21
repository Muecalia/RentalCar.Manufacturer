using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RentalCar.Manufacturer.Core.Configs;
using RentalCar.Manufacturer.Core.MessageBus;
using RentalCar.Manufacturer.Core.Services;

namespace RentalCar.Manufacturer.Infrastructure.MessageBus;

public class RabbitMqService : IRabbitMqService
{
    private RabbitMqConfig _settings { get; }
    private readonly ILoggerService _loggerService;
    
    public RabbitMqService(ILoggerService loggerService, IOptions<RabbitMqConfig> settings)
    {
        _loggerService = loggerService;
        _settings = settings.Value;
    }
    
    public async Task<IConnection> CreateConnection(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        return await factory.CreateConnectionAsync(cancellationToken);
    }

    public async Task CloseConnection(IConnection connection, IChannel channel, CancellationToken cancellationToken)
    {
        try
        {
            await channel.CloseAsync(cancellationToken);
            await channel.DisposeAsync();
            await connection.CloseAsync(cancellationToken);
            await connection.DisposeAsync();
        }
        catch (Exception ex)
        {
            _loggerService.LogInformation(MessageError.FecharConexão(ex.Message));
            throw;
        }
    }

    public async Task PublishMessage<T>(T message, string queue, CancellationToken cancellationToken)
    {
        using var connection = await CreateConnection(cancellationToken);

        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        try
        {
            await channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

            var json = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "", routingKey: queue, body: body, cancellationToken);

            _loggerService.LogInformation($"Sucesso ao publicar a mensagem na fila {queue}.");
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"Error ao publicar a mensagem. Mensagem: {ex.Message}");
            throw;
        }
        finally
        {
            await CloseConnection(connection, channel, cancellationToken);
        }
    }
}