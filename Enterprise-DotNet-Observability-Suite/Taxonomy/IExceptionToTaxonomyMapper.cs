// -----------------------------------------------------------------------
// <copyright file="IExceptionToTaxonomyMapper.cs" company="Meet Shah Builds">
// Copyright (c) 2026 Meet Shah Builds. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Enterprise_DotNet_Observability_Suite.Taxonomy;

/// <summary>
/// Represents a mapping from an exception to a standardized taxonomy.
/// </summary>
/// <param name="StatusCode">The HTTP status code to return.</param>
/// <param name="ErrorCode">The standardized error code.</param>
/// <param name="CustomMessage">An optional custom message to return.</param>
/// <param name="Details">Optional additional details about the error.</param>
public record TaxonomyMapping(
    int StatusCode,
    ErrorCode ErrorCode,
    string? CustomMessage = null,
    object? Details = null);

/// <summary>
/// Interface for mapping exceptions to standardized taxonomy mappings.
/// </summary>
public interface IExceptionToTaxonomyMapper
{
    /// <summary>
    /// Maps an exception to a standardized taxonomy mapping.
    /// </summary>
    /// <param name="exception">The exception to map.</param>
    /// <returns>A <see cref="TaxonomyMapping"/> if a mapping exists; otherwise, <c>null</c>.</returns>
    TaxonomyMapping? Map(Exception exception);
}
