namespace UserService;

public class PathResolver
{
    public static String Root = "";

    public static class Auth
    {
        public static String Base = Root + "/auth";
        public static String Register = Base +  "/register";
        public static String Login = Base + "/login";
    }

public static class User
    {
        public static String Base = Root +  "/users";
    }
}