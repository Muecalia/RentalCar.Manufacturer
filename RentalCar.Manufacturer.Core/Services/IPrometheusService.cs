namespace RentalCar.Manufacturer.Core.Services;

public interface IPrometheusService
{
    void AddNewManufacturerCounter(string statusCodes);
    void AddDeleteManufacturerCounter(string statusCodes);
    void AddUpdateManufacturerCounter(string statusCodes);
    void AddUpdateStatusManufacturerCounter(string statusCodes);
    void AddFindByIdManufacturerCounter(string statusCodes);
    void AddFindAllManufacturersCounter(string statusCodes);
}