using MediatR;

namespace SoccerManager.Application.Queries.Match.GetSubstitutionOptions;

/// <summary>
/// Represents a request to retrieve the choices a club picks a substitution's minute and situation from.
/// </summary>
public sealed record GetSubstitutionOptionsRequest : IRequest<GetSubstitutionOptionsResponse>;
