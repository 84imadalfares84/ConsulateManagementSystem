using System.Text.Json;
using System.Text.Json.Serialization;
using Consulate.Application.Common.Exceptions;
using FluentValidation;

namespace Consulate.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception for request {Path}", context.Request.Path);
            await HandleExceptionAsync(context, exception, _environment.IsDevelopment());
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        bool includeDetails)
    {
        var response = CreateResponse(context, exception, includeDetails);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        await context.Response.WriteAsync(json);
    }

    private static ErrorResponse CreateResponse(
        HttpContext context,
        Exception exception,
        bool includeDetails)
    {
        if (exception is ValidationException validationException)
        {
            return new ErrorResponse(
                400,
                "Validation failed.",
                context.TraceIdentifier,
                context.Request.Path.Value,
                validationException.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage).Distinct().ToArray()));
        }

        if (exception is AppException appException)
        {
            return new ErrorResponse(
                appException.StatusCode,
                appException.Message,
                context.TraceIdentifier,
                context.Request.Path.Value);
        }

        return new ErrorResponse(
            500,
            "An unexpected error occurred.",
            context.TraceIdentifier,
            context.Request.Path.Value,
            Details: includeDetails ? exception.Message : null);
    }

    private sealed record ErrorResponse(
        int StatusCode,
        string Message,
        string TraceId,
        string? Path,
        IDictionary<string, string[]>? Errors = null,
        string? Details = null);
}
