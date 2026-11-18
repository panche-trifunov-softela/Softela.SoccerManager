using MediatR;

namespace SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatchById;

/// <summary>
/// Represents a request to retrieve a single national team match by identifier.
/// </summary>
public sealed record GetNationalTeamMatchByIdRequest : IRequest<GetNationalTeamMatchByIdResponse>
{
    /// <summary>
    /// The identifier of the national team match to retrieve.
    /// </summary>
    public int Id { get; init; }
}
