using MediatR;

namespace SoccerManager.Application.Queries.Match.GetInstructionOptions;

/// <summary>
/// Represents a request to retrieve the instructions a club gives for a match, each with its choices.
/// </summary>
public sealed record GetInstructionOptionsRequest : IRequest<GetInstructionOptionsResponse>;
