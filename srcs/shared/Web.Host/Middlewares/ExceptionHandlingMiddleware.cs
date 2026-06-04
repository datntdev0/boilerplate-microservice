using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Shared.Common.Model;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace datntdev.Microservice.Shared.Web.Host.Middlewares;

internal class ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment environment, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly IHostEnvironment _environment = environment;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseException ex)
        {
            await HandleExceptionAsync(context, ex, _environment.IsDevelopment());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex, _environment.IsDevelopment());
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, bool includeDetails = false)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ExceptionNotFound => (int)HttpStatusCode.NotFound,
            ValidationException => (int)HttpStatusCode.BadRequest,
            ExceptionUnauthorized => (int)HttpStatusCode.Unauthorized,
            ExceptionForbidden => (int)HttpStatusCode.Forbidden,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            Details = includeDetails ? exception.StackTrace : null
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
