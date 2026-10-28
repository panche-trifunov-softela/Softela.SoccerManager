using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayerSquadNumber;

/// <summary>
/// Validates <see cref="UpdateLeagueTeamPlayerSquadNumberRequest"/> instances.
/// </summary>
public sealed class UpdateLeagueTeamPlayerSquadNumberValidator : AbstractValidator<UpdateLeagueTeamPlayerSquadNumberRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerSquadNumberValidator"/> class.
    /// </summary>
    public UpdateLeagueTeamPlayerSquadNumberValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        // The range matches the CHECK constraint on dbo.LeagueTeamPlayers, so the two agree.
        RuleFor(x => x.SquadNumber).InclusiveBetween(1, 99);
    }
}
