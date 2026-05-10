using datntdev.Microservice.Shared.Common.Exceptions;

public class ExceptionForbidden : BaseException
{
    public ExceptionForbidden() { }

    public ExceptionForbidden(string? message) : base(message) { }

    public ExceptionForbidden(string? message, Exception? innerException) : base(message, innerException) { }
}