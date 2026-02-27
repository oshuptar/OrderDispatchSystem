namespace Auth.Exceptions;

public class NotFoundException(String message) : Exception(message)
{ }