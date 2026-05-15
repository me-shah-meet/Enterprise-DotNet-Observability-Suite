// -----------------------------------------------------------------------
// <copyright file="GlobalExceptionMiddleware.cs" company="Meet Shah Builds">
// Copyright (c) 2026 Meet Shah Builds. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Enterprise_DotNet_Observability_Suite.Models;
using Enterprise_DotNet_Observability_Suite.Taxonomy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Enterprise_DotNet_Observability_Suite.Middleware;

/// <summary>
/// Centralized middleware to catch unhandled exceptions globally and return a standardized JSON error response.
/// Maps specific domain exceptions to appropriate HTTP statuses, and catches all others as ENT_SYS_9999.
/// </summary>
/// <param name="next">The request delegate.</param>
/// <param name="logger">The logger instance.</param>
public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    /// <summary>
    /// JSON serialization options for error responses.
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger logger)
    {
        context.Response.ContentType = "application/json";

        int statusCode = StatusCodes.Status500InternalServerError;
        ErrorCode errorCode = ErrorCode.SYS_9999;
        string message = "An unexpected system error occurred. Our team has been notified.";
        object? errorDetails = null;

        // Resolve any custom mappers the developers registered!
        var mappers = context.RequestServices.GetServices<IExceptionToTaxonomyMapper>();

        TaxonomyMapping? customMapping = null;
        foreach (var mapper in mappers)
        {
            customMapping = mapper.Map(ex);
            if (customMapping != null) break; // We found a match!
        }

        // Apply the custom mapping if found
        if (customMapping != null)
        {
            statusCode = customMapping.StatusCode;
            errorCode = customMapping.ErrorCode;
            message = customMapping.CustomMessage ?? ex.Message;
            errorDetails = customMapping.Details;

            logger.LogWarning(ex, "Domain exception mapped to {ErrorCode}", errorCode);
        }
        else
        {
            // Fallback for completely unhandled system crashes
            logger.LogCritical(ex, "Fatal Unhandled Exception occurred during the request.");
        }

        context.Response.StatusCode = statusCode;
        string formattedErrorCode = errorCode.ToString().Replace("_", "-");

        var response = new ErrorResponse(formattedErrorCode, message, context.TraceIdentifier, DateTime.UtcNow, errorDetails);

        var wrappedResponse = new { Success = false, Error = response, Data = (object?)null, Meta = (object?)null };
        await context.Response.WriteAsync(JsonSerializer.Serialize(wrappedResponse, JsonOptions));
    }
}