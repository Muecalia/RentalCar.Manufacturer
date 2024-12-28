namespace RentalCar.Manufacturer.Application.Commands.Request;

public class RequestValidManufacturer(string idModel, string idService)
//public class RequestValidManufacturer
{
    public string IdModel { get; set; } = idModel;
    public string IdService { get; set; } = idService;
}