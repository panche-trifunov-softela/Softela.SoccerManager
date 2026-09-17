using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Stadium.GetStadiums;

/// <summary>
/// Represents the result of a <see cref="GetStadiumsRequest"/> query.
/// </summary>
public sealed record GetStadiumsResponse
{
    /// <summary>
    /// The list of all stadiums.
    /// </summary>
    public required List<StadiumDto> Data { get; init; }
}
