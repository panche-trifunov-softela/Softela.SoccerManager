using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamNextMatch;

/// <summary>
/// Maps a next match row, its lineup and the club's instructions to a <see cref="NextMatchDto"/>.
/// </summary>
public static class GetLeagueTeamNextMatchMapper
{
    /// <summary>
    /// Converts a next match row and the club's lineup and instructions for it into the DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <param name="lineup">The club's lineup for the match.</param>
    /// <param name="instructions">The club's instructions for the match.</param>
    /// <returns>The corresponding <see cref="NextMatchDto"/>.</returns>
    public static NextMatchDto ToDto(GetNextMatchByLeagueTeamResult row, MatchLineupDto lineup, GetMatchTeamTacticInstructionsResult instructions)
    {
        return new NextMatchDto
        {
            MatchId = row.MatchId,
            StartDateTime = row.StartDateTime,
            IsHomeTeam = row.IsHomeTeam,
            OpponentLeagueTeamId = row.OpponentLeagueTeamId,
            OpponentTeamName = row.OpponentTeamName,
            OpponentTeamLogoUrl = row.OpponentTeamLogoUrl,
            Lineup = lineup,
            Instructions = new MatchInstructionsDto
            {
                MatchId = row.MatchId,
                LeagueTeamId = lineup.LeagueTeamId,
                Mentality = instructions.Mentality,
                Tempo = instructions.Tempo,
                Passing = instructions.Passing,
                Width = instructions.Width,
                Pressing = instructions.Pressing,
                Tackling = instructions.Tackling,
                AttackingSide = instructions.AttackingSide,
            },
        };
    }
}
