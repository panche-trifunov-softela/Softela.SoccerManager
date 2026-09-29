using FluentValidation;

namespace SoccerManager.Application.Commands.Player.CreatePlayer;

/// <summary>
/// Validates <see cref="CreatePlayerRequest"/> instances.
/// </summary>
public sealed class CreatePlayerValidator : AbstractValidator<CreatePlayerRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePlayerValidator"/> class.
    /// </summary>
    public CreatePlayerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

        // A missing dateOfBirth binds to DateOnly's default of 0001-01-01, so the lower bound
        // rejects an omitted value as well as a nonsensical one.
        RuleFor(x => x.DateOfBirth)
            .GreaterThan(new DateOnly(1900, 1, 1))
            .LessThan(_ => DateOnly.FromDateTime(DateTime.UtcNow));

        RuleFor(x => x.Rating).InclusiveBetween(1, 100);

        // PrecisionScale matches the DECIMAL(18,2) columns, so an over-precise value is a 400
        // rather than a silent rounding or an overflow at the database.
        RuleFor(x => x.Value).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false);
        RuleFor(x => x.Wage).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false);

        RuleFor(x => x.ImageUrl).MaximumLength(500);

        // Shape-only: that the national team exists is enforced by FK_Players_NationalTeams, and a
        // player never needs one.
        RuleFor(x => x.NationalTeamId).GreaterThan(0).When(x => x.NationalTeamId.HasValue);

        // Shape-only: that the team exists is enforced by FK_Players_Teams, and a player
        // never needs one.
        RuleFor(x => x.TeamId).GreaterThan(0).When(x => x.TeamId.HasValue);

        // Shape-only: uniqueness is enforced by UX_Players_TransfermarktId, and a player
        // never needs one.
        RuleFor(x => x.TransfermarktId).GreaterThan(0).When(x => x.TransfermarktId.HasValue);
    }
}
