namespace datntdev.Microservice.Shared.Common.Exceptions;

public class ExceptionInternal : BaseException
{
    public ExceptionInternal() { }
    public ExceptionInternal(string? message) : base(message) { }
    public ExceptionInternal(string? message, Exception? innerException) : base(message, innerException) { }
}


public class ExceptionInternalApi : ExceptionInternal
{
    public int StatusCode { get; private set; }

    public string Response { get; private set; }

    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

    public ExceptionInternalApi(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
        : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
    {
        StatusCode = statusCode;
        Response = response!;
        Headers = headers;
    }

    public override string ToString()
    {
        return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
    }
}

public partial class ExceptionInternalApi<TResult> : ExceptionInternalApi
{
    public TResult Result { get; private set; }

    public ExceptionInternalApi(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception innerException)
        : base(message, statusCode, response, headers, innerException)
    {
        Result = result;
    }
}