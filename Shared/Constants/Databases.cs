namespace Auth.Constants;

public static class Databases
{
    public static class Order
    {
        public const String DbName = "OrderDb";

        public static class Tables
        {
            public const string Orders = "Orders";
            public const string OrderDeliveries = "OrderDeliveries";
            public const string Addresses = "Addresses";
            public const string ProductionPlants = "ProductionPlants";
        }
    }

    public static class User
    {  
        public const String DbName = "UserDb";
    }
}