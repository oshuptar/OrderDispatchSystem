namespace UserService.Application.Exceptions;

public class UnauthorisedException(String message = "Unauthorised") : Exception(message);