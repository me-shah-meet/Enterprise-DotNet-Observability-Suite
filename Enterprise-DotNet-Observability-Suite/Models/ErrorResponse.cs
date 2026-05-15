// -----------------------------------------------------------------------
// <copyright file="ErrorResponse.cs" company="Meet Shah Builds">
// Copyright (c) 2026 Meet Shah Builds. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Enterprise_DotNet_Observability_Suite.Models;

/// <summary>
/// Standardized JSON response payload for API errors.
/// </summary>
public record ErrorResponse(
    string Code,
    string Message,
    string CorrelationId,
    DateTime TimestampUtc,
    object? Details = null
);
