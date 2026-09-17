using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Manager.GetManagerById;

/// <summary>
/// Represents the result of a <see cref="GetManagerByIdRequest"/> query.
/// </summary>
public sealed record GetManagerByIdResponse
{
    /// <summary>
    /// The requested manager profile.
    /// </summary>
    public required ManagerDto Data { get; init; }
}
