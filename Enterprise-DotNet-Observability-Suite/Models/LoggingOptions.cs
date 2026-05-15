// -----------------------------------------------------------------------
// <copyright file="LoggingOptions.cs" company="Meet Shah Builds">
// Copyright (c) 2026 Meet Shah Builds. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Enterprise_DotNet_Observability_Suite.Models;

/// <summary>
/// Configuration options for Enterprise centralized logging.
/// </summary>
public class LoggingOptions
{
    /// <summary>
    /// The configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "Logging";

    /// <summary>
    /// The URL for the Seq logging server.
    /// </summary>
    public string? SeqUrl { get; set; }

    /// <summary>
    /// The API key for the Seq logging server.
    /// </summary>
    public string? SeqApiKey { get; set; }

    /// <summary>
    /// The connection string for the PostgreSQL logging database.
    /// </summary>
    public string? PostgresConnectionString { get; set; }

    /// <summary>
    /// The default minimum log level (e.g., Information, Warning, Error).
    /// </summary>
    public string DefaultLogLevel { get; set; } = "Information";

    /// <summary>
    /// The PostgreSQL schema where the logs table will be created. Defaults to "public".
    /// </summary>
    public string SchemaName { get; set; } = "public";
}