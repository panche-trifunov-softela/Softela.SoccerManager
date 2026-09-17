using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Stadium.GetStadiumById;

/// <summary>
/// Represents the result of a <see cref="GetStadiumByIdRequest"/> query.
/// </summary>
public sealed record GetStadiumByIdResponse
{
    /// <summary>
    /// The requested stadium.
    /// </summary>
    public required StadiumDto Data { get; init; }
}
