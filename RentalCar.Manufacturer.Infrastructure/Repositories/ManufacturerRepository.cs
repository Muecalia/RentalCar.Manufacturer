using Microsoft.EntityFrameworkCore;
using RentalCar.Manufacturer.Core.Entities;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Infrastructure.Persistence;

namespace RentalCar.Manufacturer.Infrastructure.Repositories;

public class ManufacturerRepository : IManufacturerRepository
{
    private readonly ManufacturerContext _context;

    public ManufacturerRepository(ManufacturerContext context)
    {
        _context = context;
    }

    public async Task<Manufacturers> Create(Manufacturers manufacturer, CancellationToken cancellationToken)
    {
        _context.Manufacturer.Add(manufacturer);
        await _context.SaveChangesAsync(cancellationToken);
        return manufacturer;
    }

    public async Task Delete(Manufacturers manufacturer, CancellationToken cancellationToken)
    {
        manufacturer.DeletedAt = DateTime.UtcNow;
        _context.Update(manufacturer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Manufacturers>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Manufacturer
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Manufacturers?> GetById(string id, CancellationToken cancellationToken)
    {
        return await _context.Manufacturer.FirstOrDefaultAsync(c => !c.IsDeleted && string.Equals(c.Id, id), cancellationToken);
    }

    public async Task<bool> IsManufacturerExist(string name, CancellationToken cancellationToken)
    {
        return await _context.Manufacturer.AnyAsync(c => string.Equals(c.Name, name), cancellationToken);
    }

    public async Task<bool> IsEmailExist(string email, CancellationToken cancellationToken)
    {
        return await _context.Manufacturer.AnyAsync(c => string.Equals(c.Email, email), cancellationToken);
    }

    public async Task Update(Manufacturers manufacturer, CancellationToken cancellationToken)
    {
        manufacturer.UpdatedAt = DateTime.UtcNow;
        _context.Manufacturer.Update(manufacturer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}