using SoccerManager.Application.Core.Extensions;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSchedule;

/// <summary>
/// Maps schedule rows to <see cref="LeagueTeamScheduleMatchDto"/> instances.
/// </summary>
public static class GetLeagueTeamScheduleMapper
{
    /// <summary>
    /// Converts a schedule row into its DTO representation. The score and the outcome are set only for a finished match,
    /// because an unplayed match reads as 0-0 in the row.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeagueTeamScheduleMatchDto"/>.</returns>
    public static LeagueTeamScheduleMatchDto ToDto(GetScheduleByLeagueTeamResult row)
    {
        var outcome = GetOutcome(row);

        return new LeagueTeamScheduleMatchDto
        {
            MatchId = row.MatchId,
            StartDateTime = row.StartDateTime,
            IsHomeTeam = row.IsHomeTeam,
            CompetitionName = row.CompetitionName,
            StageName = GetStageName(row),
            OpponentTeamName = row.OpponentTeamName,
            OpponentTeamLogoUrl = row.OpponentTeamLogoUrl,
            GoalsFor = row.IsFinished ? row.GoalsFor : null,
            GoalsAgainst = row.IsFinished ? row.GoalsAgainst : null,
            ShootoutGoalsFor = row.IsFinished ? row.ShootoutGoalsFor : null,
            ShootoutGoalsAgainst = row.IsFinished ? row.ShootoutGoalsAgainst : null,
            Outcome = outcome,
            OutcomeLabel = outcome?.ToString().ToWords(),
        };
    }

    /// <summary>
    /// Works out how the match ended for the club.
    /// </summary>
    /// <param name="row">The schedule row.</param>
    /// <returns>The outcome, or <see langword="null"/> until the match is finished.</returns>
    private static MatchOutcome? GetOutcome(GetScheduleByLeagueTeamResult row)
    {
        if (!row.IsFinished)
        {
            return null;
        }

        if (row.GoalsFor != row.GoalsAgainst)
        {
            return row.GoalsFor > row.GoalsAgainst ? MatchOutcome.Win : MatchOutcome.Loss;
        }

        // A level score is decided by the shoot-out when there was one and it was not level; a level shoot-out or none is a draw.
        if (row.ShootoutGoalsFor is { } shootoutFor
            && row.ShootoutGoalsAgainst is { } shootoutAgainst
            && shootoutFor != shootoutAgainst)
        {
            return shootoutFor > shootoutAgainst ? MatchOutcome.Win : MatchOutcome.Loss;
        }

        return MatchOutcome.Draw;
    }

    /// <summary>
    /// Names the round of the match in words, with the leg of a two-legged tie.
    /// </summary>
    /// <param name="row">The schedule row.</param>
    /// <returns>The round in words, or <see langword="null"/> when the match has none to name.</returns>
    private static string? GetStageName(GetScheduleByLeagueTeamResult row)
    {
        if (row.Stage is null)
        {
            return null;
        }

        var words = (MatchStage)row.Stage.Value switch
        {
            MatchStage.DomesticCupRound => row.RoundNumber is null ? null : $"Round {row.RoundNumber}",
            MatchStage.PromotionPlayoffSemiFinal => "Play-off semi-final",
            MatchStage.PromotionPlayoffFinal => "Play-off final",
            MatchStage.ContinentalLeaguePhase => row.RoundNumber is null ? "League phase" : $"League phase, matchday {row.RoundNumber}",
            MatchStage.ContinentalKnockoutPlayOff => "Knockout play-off",
            MatchStage.ContinentalRoundOf16 => "Round of 16",
            MatchStage.ContinentalQuarterFinal => "Quarter-final",
            MatchStage.ContinentalSemiFinal => "Semi-final",
            MatchStage.ContinentalFinal => "Final",

            // The World Cup stages are not named on a club's schedule.
            _ => null,
        };

        if (words is null)
        {
            return null;
        }

        return row.Leg switch
        {
            1 => $"{words}, 1st leg",
            2 => $"{words}, 2nd leg",
            _ => words,
        };
    }
}
