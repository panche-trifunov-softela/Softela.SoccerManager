using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Season.GetSeasonById;

/// <summary>
/// Represents the result of a <see cref="GetSeasonByIdRequest"/> query.
/// </summary>
public sealed record GetSeasonByIdResponse
{
    /// <summary>
    /// The requested season.
    /// </summary>
    public required SeasonDto Data { get; init; }
}
