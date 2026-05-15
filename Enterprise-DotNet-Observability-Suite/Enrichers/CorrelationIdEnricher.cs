// -----------------------------------------------------------------------
// <copyright file="CorrelationIdEnricher.cs" company="Meet Shah Builds">
// Copyright (c) 2026 Meet Shah Builds. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Enterprise_DotNet_Observability_Suite.Enrichers;

/// <summary>
/// Serilog enricher that extracts the Correlation ID from the active HTTP context.
/// </summary>
/// <param name="httpContextAccessor">The HTTP context accessor.</param>
public class CorrelationIdEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    /// <summary>
    /// The name of the Correlation ID property in the log event.
    /// </summary>
    private const string CorrelationIdPropertyName = "CorrelationId";

    /// <summary>
    /// The name of the HTTP header that carries the Correlation ID.
    /// </summary>
    private const string CorrelationIdHeaderName = "X-Correlation-ID";

    /// <summary>
    /// Enriches the log event with the dynamically resolved Correlation ID.
    /// </summary>
    /// <param name="logEvent">The current log event.</param>
    /// <param name="propertyFactory">Factory to create new log properties.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var correlationId = GetCorrelationId();
        var correlationProperty = propertyFactory.CreateProperty(CorrelationIdPropertyName, correlationId);
        logEvent.AddPropertyIfAbsent(correlationProperty);
    }

    /// <summary>
    /// Retrieves the Correlation ID from the current HTTP context or generates a new one if unavailable.
    /// </summary>
    /// <returns>The Correlation ID as a string.</returns>
    private string GetCorrelationId()
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null) return Guid.NewGuid().ToString("N");

        if (context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var headerValue))
        {
            return headerValue.ToString();
        }

        return context.TraceIdentifier;
    }
}