using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.SearchLeaguePlayersAndTeams;

/// <summary>
/// Validates <see cref="SearchLeaguePlayersAndTeamsRequest"/> instances.
/// </summary>
public sealed class SearchLeaguePlayersAndTeamsValidator : AbstractValidator<SearchLeaguePlayersAndTeamsRequest>
{
    /// <summary>
    /// The fewest characters a search term must have once trimmed.
    /// </summary>
    private const int MinTermLength = 2;

    /// <summary>
    /// The most characters a search term may have. It matches the NVARCHAR(100) Name columns, so the procedure parameter
    /// never cuts a longer term silently.
    /// </summary>
    private const int MaxTermLength = 100;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchLeaguePlayersAndTeamsValidator"/> class.
    /// </summary>
    public SearchLeaguePlayersAndTeamsValidator()
    {
        // A missing, zero or negative id fails fast here, as a 400, before the access check, where it would be a 403.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);

        // One message covers a missing, blank and too short term, and the first failure stops the rules after it. A term of
        // one letter would match most of the catalogue, so it is not searched. The minimum is counted once trimmed, as the
        // mapper trims; the maximum is counted as sent, which is stricter and never cuts the procedure parameter.
        RuleFor(x => x.SearchTerm)
            .Cascade(CascadeMode.Stop)
            .Must(term => term is not null && term.Trim().Length >= MinTermLength)
            .WithMessage($"The search term must be at least {MinTermLength} characters.")
            .MaximumLength(MaxTermLength);
    }
}
