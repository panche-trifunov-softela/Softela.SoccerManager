using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Services;

/// <summary>
/// Holds the rules a transfer, a loan and a swap share, so each is checked by one definition: the clubs and the season
/// of a deal must exist and belong to the player's league, and a swapped player must be one the buying club can give away.
/// </summary>
public class PlayerDealService : IPlayerDealService
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly ILeagueTeamPlayerTransferRepository _leagueTeamPlayerTransferRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerDealService"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the two clubs.</param>
    /// <param name="seasonRepository">The repository used to load the season.</param>
    /// <param name="leagueTeamPlayerTransferRepository">The repository used to load the transfer a player is swapped in.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to load the swapped player's squad row.</param>
    public PlayerDealService(ILeagueTeamRepository leagueTeamRepository, ISeasonRepository seasonRepository, ILeagueTeamPlayerTransferRepository leagueTeamPlayerTransferRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _seasonRepository = seasonRepository;
        _leagueTeamPlayerTransferRepository = leagueTeamPlayerTransferRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
    }

    /// <summary>
    /// Ensures a deal (a transfer or a loan) moves a player to another club of the player's own league, in a season of that
    /// league. It runs for every caller, operators included, because nothing else keeps a deal inside one league.
    /// </summary>
    /// <param name="ownLeagueTeamId">The identifier of the league team (the club in its league) the player is registered at.</param>
    /// <param name="toLeagueTeamId">The identifier of the league team (the club in its league) the player goes to.</param>
    /// <param name="seasonId">The identifier of the season the deal takes place in.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the player's club, the destination club or the season does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the destination is the player's own club, a club of another league, or the season belongs to another league.</exception>
    public async Task EnsureDealIsValidAsync(int ownLeagueTeamId, int toLeagueTeamId, int seasonId)
    {
        var ownLeagueTeam = await _leagueTeamRepository.GetByIdAsync(ownLeagueTeamId)
            ?? throw new KeyNotFoundException($"League team {ownLeagueTeamId} not found.");

        var toLeagueTeam = await _leagueTeamRepository.GetByIdAsync(toLeagueTeamId)
            ?? throw new KeyNotFoundException($"League team {toLeagueTeamId} not found.");

        var season = await _seasonRepository.GetByIdAsync(seasonId)
            ?? throw new KeyNotFoundException($"Season {seasonId} not found.");

        var failures = new List<ValidationFailure>();

        if (toLeagueTeamId == ownLeagueTeamId)
        {
            failures.Add(new ValidationFailure(nameof(LeagueTeamPlayerTransfer.ToLeagueTeamId), "The destination club cannot be the player's own club."));
        }
        else if (toLeagueTeam.LeagueId != ownLeagueTeam.LeagueId)
        {
            failures.Add(new ValidationFailure(nameof(LeagueTeamPlayerTransfer.ToLeagueTeamId), "The destination club must be in the player's league."));
        }

        if (season.LeagueId != ownLeagueTeam.LeagueId)
        {
            failures.Add(new ValidationFailure(nameof(LeagueTeamPlayerTransfer.SeasonId), "The season must be in the player's league."));
        }

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }
    }

    /// <summary>
    /// Ensures a swapped player can go with a transfer: he is not the player the transfer sells, and he is in the squad of
    /// the buying club. It runs for every caller, operators included, because nothing else checks a swap.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferId">The identifier of the transfer the player is swapped in.</param>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row (the player at a club of the league) that is swapped.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the transfer or the swapped player's squad row does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the swapped player is the sold player, or is not in the buying club's squad.</exception>
    public async Task EnsureSwapIsValidAsync(int leagueTeamPlayerTransferId, int leagueTeamPlayerId)
    {
        var transfer = await _leagueTeamPlayerTransferRepository.GetByIdAsync(leagueTeamPlayerTransferId)
            ?? throw new KeyNotFoundException($"League team player transfer {leagueTeamPlayerTransferId} not found.");

        var squadRow = await _leagueTeamPlayerRepository.GetByIdAsync(leagueTeamPlayerId)
            ?? throw new KeyNotFoundException($"League team player {leagueTeamPlayerId} not found.");

        // The sold player cannot be swapped back, and a swapped player must be one the buying club can give away.
        if (squadRow.Id == transfer.LeagueTeamPlayerId)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(LeagueTeamPlayerTransferSwap.LeagueTeamPlayerId), "The sold player cannot also be a swapped player."),
            });
        }

        if (squadRow.LeagueTeamId != transfer.ToLeagueTeamId)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(LeagueTeamPlayerTransferSwap.LeagueTeamPlayerId), "The swapped player must be in the buying club's squad."),
            });
        }
    }
}
