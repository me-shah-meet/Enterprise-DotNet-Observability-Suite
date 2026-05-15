// -----------------------------------------------------------------------
// <copyright file="DependencyInjectionExtensions.cs" company="Meet Shah Builds">
// Copyright (c) 2026 Meet Shah Builds. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Enterprise_DotNet_Observability_Suite.Behaviours;
using Enterprise_DotNet_Observability_Suite.Enrichers;
using Enterprise_DotNet_Observability_Suite.Middleware;
using Enterprise_DotNet_Observability_Suite.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Enterprise_DotNet_Observability_Suite.Extensions;


/// <summary>
/// Extension methods for bootstrapping the logging and observability pipeline.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// The ultimate bootstrapper for Observability. 
    /// Clears default providers, injects CQRS logging, and configures Serilog.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder for chaining.</returns>
    public static WebApplicationBuilder AddEnterpriseObservability(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Services.AddEnterpriseCqrsLogging();
        builder.Host.UseEnterpriseSerilog();
        return builder;
    }

    /// <summary>
    /// Registers the CQRS Logging Behavior and necessary context accessors.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddEnterpriseCqrsLogging(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddSingleton<CorrelationIdEnricher>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }

    /// <summary>
    /// Configures Serilog with Enterprise opinionated defaults (Console, Seq, PostgreSQL).
    /// </summary>
    /// <param name="hostBuilder">The host builder.</param>
    /// <returns>The modified host builder.</returns>
    public static IHostBuilder UseEnterpriseSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, configuration) =>
        {
            var options = new LoggingOptions();
            context.Configuration.GetSection(LoggingOptions.SectionName).Bind(options);

            var minLevel = Enum.TryParse<LogEventLevel>(options.DefaultLogLevel, out var level)
                ? level
                : LogEventLevel.Information;

            configuration
                .MinimumLevel.Is(minLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.With(services.GetRequiredService<CorrelationIdEnricher>())
                .Enrich.With<ServiceContextEnricher>()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] [{Service}] {Message:lj}{NewLine}{Exception}");

            if (!string.IsNullOrWhiteSpace(options.SeqUrl))
            {
                configuration.WriteTo.Seq(options.SeqUrl, apiKey: options.SeqApiKey);
            }

            if (!string.IsNullOrWhiteSpace(options.PostgresConnectionString))
            {
                configuration.WriteTo.PostgreSQL(
                    connectionString: options.PostgresConnectionString,
                    tableName: "api_logs",
                    schemaName: options.SchemaName,
                    needAutoCreateTable: true);
            }
        });
    }

    /// <summary>
    /// Adds the Global Exception Middleware to the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The IApplicationBuilder.</param>
    /// <returns>The modified IApplicationBuilder.</returns>
    public static IApplicationBuilder UseEnterpriseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
