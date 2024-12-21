using RabbitMQ.Client;

namespace RentalCar.Manufacturer.Core.Services;

public interface IRabbitMqService
{
    Task<IConnection> CreateConnection(CancellationToken cancellationToken);
    Task CloseConnection(IConnection connection, IChannel channel, CancellationToken cancellationToken);
    Task PublishMessage<T>(T message, string queue, CancellationToken cancellationToken);
}