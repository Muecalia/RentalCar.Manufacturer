namespace RentalCar.Manufacturer.Core.MessageBus;

public class RabbitQueue
{
    public static string MANUFACTURER_MODEL_NEW_REQUEST_QUEUE = "ManufacturerModelNewRequestQueue";
    public static string MANUFACTURER_MODEL_NEW_RESPONSE_QUEUE = "ManufacturerModelNewResponseQueue";
    
    public static string MANUFACTURER_MODEL_FIND_REQUEST_QUEUE = "ManufacturerModelFindRequestQueue";
    public static string MANUFACTURER_MODEL_FIND_RESPONSE_QUEUE = "ManufacturerModelFindResponseQueue";
    
    public static string MANUFACTURER_MODEL_UPDATE_REQUEST_QUEUE = "ManufacturerModelUpdateRequestQueue";
    public static string MANUFACTURER_MODEL_UPDATE_RESPONSE_QUEUE = "ManufacturerModelUpdateResponseQueue";
}