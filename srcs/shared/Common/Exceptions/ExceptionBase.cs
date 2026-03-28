namespace datntdev.Microservice.Shared.Common.Exceptions;

public class ExceptionBase : Exception
{
    public ExceptionBase() { }

    public ExceptionBase(string? message) : base(message) { }

    public ExceptionBase(string? message, Exception? innerException) : base(message, innerException) { }
}