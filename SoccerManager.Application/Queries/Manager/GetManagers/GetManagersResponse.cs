using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Manager.GetManagers;

/// <summary>
/// Represents the result of a <see cref="GetManagersRequest"/> query.
/// </summary>
public sealed record GetManagersResponse
{
    /// <summary>
    /// The list of all managers.
    /// </summary>
    public required List<ManagerDto> Data { get; init; }
}
