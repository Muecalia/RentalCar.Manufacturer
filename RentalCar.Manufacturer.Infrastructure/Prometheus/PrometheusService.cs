using RentalCar.Manufacturer.Core.Services;

namespace RentalCar.Manufacturer.Infrastructure.Prometheus;

public class PrometheusService : IPrometheusService
{
    //private static readonly Counter RequestAccountCounter = Metrics.CreateCounter("account_total", "Total requisições de criação de conta", ["status_code"]);
    //private static readonly Counter RequestRoleCounter = Metrics.CreateCounter("role_total", "Total requisições de criação de perfil", ["status_code"]);
    //private static readonly Counter RequestLoginCounter = Metrics.CreateCounter("login_total", "Total requisições de login (acesso dos utilizadores)", ["status_code"]);
    //private static readonly Counter RequestLoginCounter = Metrics.CreateCounter("login_total", "Total requisições de login (acesso dos utilizadores)", ["status_code"]);
    
    

    public void AddNewManufacturerCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddDeleteManufacturerCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddUpdateManufacturerCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddUpdateStatusManufacturerCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddFindByIdManufacturerCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddFindAllManufacturersCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }
    
}