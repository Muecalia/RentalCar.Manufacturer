namespace RentalCar.Manufacturer.Core.MessageBus;

public class RabbitQueue
{
    //ACCOUNT
    public static string NEW_ACCOUNT_QUEUE = "NewAccountQueue";
    public static string UPDATE_ACCOUNT_QUEUE = "UpdateAccountQueue";
    public static string UPDATE_STATUS_ACCOUNT_QUEUE = "UpdateStatusAccountQueue";

    //CLIENT
    public static string NEW_CLIENT_QUEUE = "NewClienttQueue";
    public static string UPDATE_CLIENT_QUEUE = "UpdateClienttQueue";
    
    public static string CLIENT_REQUEST_QUEUE = "ClientRequestQueue";
    public static string CLIENT_RESPONSE_QUEUE = "ClientResponseQueue";
}