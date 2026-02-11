namespace UserService.Application.Common.Exceptions;

public class NotFoundException(String message) : Exception(message)
{ }