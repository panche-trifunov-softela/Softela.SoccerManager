using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquadMembers;

/// <summary>
/// Validates <see cref="GetLeagueTeamSquadMembersRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamSquadMembersValidator : AbstractValidator<GetLeagueTeamSquadMembersRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamSquadMembersValidator"/> class.
    /// </summary>
    public GetLeagueTeamSquadMembersValidator()
    {
        // This query is always scoped to a league team, so a missing or zero LeagueTeamId must fail fast here, as a 400,
        // rather than reach the handler, where it would be a 403 or an empty list.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
