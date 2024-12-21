using RentalCar.Manufacturer.Core.Entities;

namespace RentalCar.Manufacturer.Core.Repositories;

public interface IManufacturerRepository
{
    Task<Manufacturers> Create(Manufacturers manufacturer, CancellationToken cancellationToken);
    Task Update(Manufacturers manufacturer, CancellationToken cancellationToken);
    Task Delete(Manufacturers manufacturer, CancellationToken cancellationToken);
    Task<bool> IsManufacturerExist(string name, CancellationToken cancellationToken);
    Task<bool> IsEmailExist(string email, CancellationToken cancellationToken);
    Task<Manufacturers?> GetById(string id, CancellationToken cancellationToken);
    Task<List<Manufacturers>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);
}