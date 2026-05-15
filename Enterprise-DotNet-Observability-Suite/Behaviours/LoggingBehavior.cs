// -----------------------------------------------------------------------
// <copyright file="LoggingBehavior.cs" company="Meet Shah Builds">
// Copyright (c) 2026 Meet Shah Builds. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Enterprise_DotNet_Observability_Suite.Behaviours;

/// <summary>
/// A MediatR pipeline behavior that automatically logs the execution and duration of Commands and Queries.
/// </summary>
/// <typeparam name="TRequest">The type of the CQRS request.</typeparam>
/// <typeparam name="TResponse">The type of the CQRS response.</typeparam>
/// <param name="logger">The logger instance.</param>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    /// <summary>
    /// Handles the logging interception for the given request.
    /// </summary>
    /// <param name="request">The incoming MediatR request.</param>
    /// <param name="next">The delegate to the next handler in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response from the handler.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Starting CQRS Request: {RequestName}", requestName);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next();
            stopwatch.Stop();
            logger.LogInformation("Completed CQRS Request: {RequestName} in {Duration}ms", requestName, stopwatch.ElapsedMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex, "Failed CQRS Request: {RequestName} after {Duration}ms", requestName, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
