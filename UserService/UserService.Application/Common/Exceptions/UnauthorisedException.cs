namespace UserService.Application.Common.Exceptions;

public class UnauthorisedException(String message = "Unauthorised") : Exception(message);