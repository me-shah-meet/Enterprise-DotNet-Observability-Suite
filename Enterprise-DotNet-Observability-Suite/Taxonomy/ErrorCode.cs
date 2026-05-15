// -----------------------------------------------------------------------
// <copyright file="ErrorCode.cs" company="Meet Shah Builds">
// Copyright (c) 2026 Meet Shah Builds. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Enterprise_DotNet_Observability_Suite.Taxonomy;

/// <summary>
/// Standardized error codes for the Enterprise ecosystem, approved by client.
/// </summary>
public enum ErrorCode
{
    /// <summary>Unknown or unclassified error.</summary>
    UnknownError = 0,

    // --- 1. Global System & Infrastructure Errors (SYS) ---
    /// <summary>Fatal Unhandled Exception: A completely unexpected system crash.</summary>
    SYS_9999 = 50000,
    /// <summary>Database Unavailable: Failure to connect to PostgreSQL or Redis.</summary>
    SYS_E001 = 50301,
    /// <summary>Internal Timeout: A cross-service HTTP call timed out.</summary>
    SYS_E002 = 50401,
    /// <summary>Validation: Malformed Request: The incoming JSON body is structurally invalid.</summary>
    SYS_V001 = 40001,
    /// <summary>Validation: Rate Limit Exceeded.</summary>
    SYS_V002 = 42901,
    /// <summary>Not Found: Generic Resource Not Found.</summary>
    SYS_N001 = 40400,
    /// <summary>Conflict: Generic Resource Conflict.</summary>
    SYS_C001 = 40900,
    /// <summary>Business: Generic Business Rule Violation.</summary>
    SYS_B001 = 42200,
}
