namespace Auth.Constants;

public class PathResolver
{
    private static readonly String Root = "";

    public static class Auth
    {
        public static readonly String Base = Root + "/auth";
        public static readonly String Register = Base + "/register";
        public static readonly String Login = Base + "/login";
    }

    public static class Users
    {
        public static readonly String Base = Root +  "/users";
    }

    public static class Orders
    {
        public static readonly String Base = Root + "/orders";
    }
}