namespace datntdev.Microservice.Shared.Common.Exceptions;

public class ExceptionUnauthorized : BaseException
{
    public ExceptionUnauthorized() { }

    public ExceptionUnauthorized(string? message) : base(message) { }

    public ExceptionUnauthorized(string? message, Exception? innerException) : base(message, innerException) { }

    public static ExceptionUnauthorized Default() => new("Authentication is required to access this resource.");
}