using MediatR;

namespace SoccerManager.Application.Queries.Player.GetPlayers;

/// <summary>
/// Represents a request to retrieve all players.
/// </summary>
public sealed record GetPlayersRequest : IRequest<GetPlayersResponse>;
