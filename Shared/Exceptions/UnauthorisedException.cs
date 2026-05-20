namespace Auth.Exceptions;

public class UnauthorisedException(String message = "Unauthorised") : Exception(message);