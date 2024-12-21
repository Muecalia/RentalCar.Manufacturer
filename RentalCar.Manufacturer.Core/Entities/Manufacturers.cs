namespace RentalCar.Manufacturer.Core.Entities;

public class Manufacturers
{
    public Manufacturers()
    {
        IsDeleted = false;
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
    }

    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Email { get; set; }
}