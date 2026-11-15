using FluentValidation;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Core.Extensions;

/// <summary>
/// Validation rules that keep guest clubs out of being managed and out of divisions. A guest is on a league's roster so its
/// players can be bought, but it is never managed and never plays in a division.
/// </summary>
public static class GuestClubRuleExtensions
{
    /// <summary>
    /// The message reported when a guest club is appointed a manager, applied for or accepted for.
    /// </summary>
    public const string GuestClubMessage = "The club is a guest of this league and cannot be managed.";

    /// <summary>
    /// The message reported when a guest club is given a standing in a division.
    /// </summary>
    public const string GuestInDivisionMessage = "A guest club cannot be in a division.";

    /// <summary>
    /// Requires the roster entry not to be a guest; an unknown roster entry passes, because the other rules and the foreign key own that case.
    /// </summary>
    /// <typeparam name="T">The type of the request being validated.</typeparam>
    /// <param name="ruleBuilder">The rule for the roster entry's identifier.</param>
    /// <param name="leagueTeamRepository">The repository used to look up the roster entry.</param>
    /// <returns>The rule's options, for further configuration.</returns>
    public static IRuleBuilderOptions<T, int> MustNotBeGuest<T>(this IRuleBuilder<T, int> ruleBuilder, ILeagueTeamRepository leagueTeamRepository)
    {
        return ruleBuilder
            .MustAsync(async (leagueTeamId, _) =>
            {
                var leagueTeam = await leagueTeamRepository.GetByIdAsync(leagueTeamId);

                return leagueTeam is null || !leagueTeam.IsGuest;
            })
            .WithMessage(GuestClubMessage);
    }

    /// <summary>
    /// Requires a guest club not to be placed in a division, that is a competition whose order is above zero. A cup, whose order
    /// is zero, stays allowed, and an unknown roster entry or competition passes, because the other rules and the foreign keys own those cases.
    /// </summary>
    /// <typeparam name="T">The type of the request being validated.</typeparam>
    /// <param name="ruleBuilder">The rule for the whole request, from <c>RuleFor(x => x)</c>.</param>
    /// <param name="competitionId">Selects the competition's identifier from the request.</param>
    /// <param name="leagueTeamId">Selects the roster entry's identifier from the request.</param>
    /// <param name="competitionRepository">The repository used to look up the competition.</param>
    /// <param name="leagueTeamRepository">The repository used to look up the roster entry.</param>
    /// <returns>The rule's options, for further configuration.</returns>
    public static IRuleBuilderOptions<T, T> MustNotBePlacedInDivision<T>(this IRuleBuilder<T, T> ruleBuilder, Func<T, int> competitionId, Func<T, int> leagueTeamId, ICompetitionRepository competitionRepository, ILeagueTeamRepository leagueTeamRepository)
    {
        // The roster entry is read first, so a club that is not a guest never costs a competition read.
        return ruleBuilder
            .MustAsync(async (request, _) =>
            {
                var leagueTeam = await leagueTeamRepository.GetByIdAsync(leagueTeamId(request));

                if (leagueTeam is null || !leagueTeam.IsGuest)
                {
                    return true;
                }

                var competition = await competitionRepository.GetByIdAsync(competitionId(request));

                return competition is null || competition.Order == 0;
            })
            .OverridePropertyName("LeagueTeamId")
            .WithMessage(GuestInDivisionMessage);
    }
}
