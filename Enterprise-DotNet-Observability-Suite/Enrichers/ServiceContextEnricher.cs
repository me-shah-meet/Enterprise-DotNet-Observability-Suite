// -----------------------------------------------------------------------
// <copyright file="ServiceContextEnricher.cs" company="Meet Shah Builds">
// Copyright (c) 2026 Meet Shah Builds. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Serilog.Core;
using Serilog.Events;
using System.Reflection;

namespace Enterprise_DotNet_Observability_Suite.Enrichers;

/// <summary>
/// Serilog enricher that injects the executing assembly name and environment variable into logs.
/// </summary>
public class ServiceContextEnricher : ILogEventEnricher
{
    /// <summary>
    /// Enriches the log event with Service and Environment properties.
    /// </summary>
    /// <param name="logEvent">The current log event.</param>
    /// <param name="propertyFactory">Factory to create new log properties.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var serviceName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown Service";
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Service", serviceName));

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Environment", env));
    }
}
