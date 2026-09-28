using MediatR;

namespace SoccerManager.Application.Queries.NationalTeam.GetNationalTeamById;

/// <summary>
/// Represents a request to retrieve a single national team by identifier.
/// </summary>
public sealed record GetNationalTeamByIdRequest : IRequest<GetNationalTeamByIdResponse>
{
    /// <summary>
    /// The identifier of the national team to retrieve.
    /// </summary>
    public int Id { get; init; }
}
