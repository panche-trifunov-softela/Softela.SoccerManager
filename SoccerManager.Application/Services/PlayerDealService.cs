using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Services;

/// <summary>
/// Holds the rules a transfer, a loan and a swap share, so each is checked by one definition: the clubs and the season
/// of a deal must exist and belong to the player's league, a swapped player must be one the buying club can give away,
/// and a club's bid must be for a player of another club of its league, in the league's latest season, while the club
/// has no other pending bid for that player and the player, like every player offered in a swap, has no agreed deal.
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
    /// league. Either club may be a guest. It runs for every caller, operators included, because nothing else keeps a deal
    /// inside one league.
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
    /// the buying club. Either club may be a guest. It runs for every caller, operators included, because nothing else checks
    /// a swap.
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

    /// <summary>
    /// Resolves what a club's bid for a player is aimed at, and ensures the bid can be placed: the player is not in the
    /// bidding club's squad, is registered at another club of the league, the league has a season to bid in, the club has
    /// no pending bid for the player yet, and the player has no agreed deal. The bid is placed in the league's latest
    /// season. Either club may be a guest. It runs for every caller, operators included, and takes a lock on the rows of
    /// the bidding club and of the selling club, so it belongs inside the caller's transaction.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) that bids.</param>
    /// <param name="playerId">The identifier of the player the club bids for.</param>
    /// <returns>The resolved target of the bid.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bidding club does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the player is already in the bidding club's squad, is registered at no club of the league, the league has no season yet, the club already has a pending bid for the player, or the player already has an agreed deal.</exception>
    public async Task<BidTarget> GetBidTargetAsync(int leagueTeamId, int playerId)
    {
        var leagueTeam = await _leagueTeamRepository.GetByIdAsync(leagueTeamId)
            ?? throw new KeyNotFoundException($"League team {leagueTeamId} not found.");

        // Every registration counts, so a player the club holds twice is still its own.
        var squad = await _leagueTeamPlayerRepository.GetByLeagueTeamIdAsync(leagueTeamId);

        if (squad.Any(member => member.PlayerId == playerId))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(LeagueTeamPlayer.PlayerId), "The player is already in the club's squad."),
            });
        }

        var sellerRow = await _leagueTeamPlayerRepository.GetByLeagueAndPlayerAsync(leagueTeam.LeagueId, playerId);

        if (sellerRow is null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(LeagueTeamPlayer.PlayerId), "The player is not registered at any club of this league."),
            });
        }

        var season = await _seasonRepository.GetLatestByLeagueIdAsync(leagueTeam.LeagueId);

        if (season is null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(LeagueTeamPlayer.LeagueTeamId), "The league has no season yet."),
            });
        }

        // The check takes the lock on the rows of the bidding club and of the selling club, so a second bid from the club, and
        // an accept on the player, wait for this transaction.
        if (await _leagueTeamRepository.HasPendingBidAsync(leagueTeamId, sellerRow.LeagueTeamId, playerId))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(LeagueTeamPlayer.PlayerId), "The club already has a pending bid for this player."),
            });
        }

        // A player whose deal is agreed takes no new bid. The clubs are locked by now, so no accept can change the answer.
        if (await _leagueTeamRepository.HasAgreedDealAsync(sellerRow.Id))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(LeagueTeamPlayer.PlayerId), "This player already has an agreed deal."),
            });
        }

        return new BidTarget
        {
            LeagueTeamPlayerId = sellerRow.Id,
            FromLeagueTeamId = sellerRow.LeagueTeamId,
            SeasonId = season.Id,
            SquadLeagueTeamPlayerIds = squad.Select(member => member.Id).ToList(),
        };
    }

    /// <summary>
    /// Ensures none of the players offered in a swap already has an agreed deal. It takes no lock of its own, so it belongs
    /// inside the caller's transaction after <see cref="GetBidTargetAsync"/>, which has locked the bidding club the
    /// swapped players belong to.
    /// </summary>
    /// <param name="leagueTeamPlayerIds">The identifiers of the squad rows (the players at the bidding club) that are offered in the swap.</param>
    /// <exception cref="ValidationException">Thrown when a swapped player already has an agreed deal.</exception>
    public async Task EnsureSwapPlayersHaveNoAgreedDealAsync(IReadOnlyCollection<int> leagueTeamPlayerIds)
    {
        foreach (var leagueTeamPlayerId in leagueTeamPlayerIds)
        {
            if (await _leagueTeamRepository.HasAgreedDealAsync(leagueTeamPlayerId))
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(LeagueTeamPlayerTransferSwap.LeagueTeamPlayerId), "A swapped player already has an agreed deal."),
                });
            }
        }
    }
}
