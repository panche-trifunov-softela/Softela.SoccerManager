using FluentValidation;

namespace SoccerManager.Application.Commands.Manager.UpdateManager;

/// <summary>
/// Validates <see cref="UpdateManagerRequest"/> instances.
/// </summary>
public sealed class UpdateManagerValidator : AbstractValidator<UpdateManagerRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateManagerValidator"/> class.
    /// </summary>
    public UpdateManagerValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.ImageUrl).MaximumLength(500);

        // A null image stays allowed, as it clears the image; any other value is rendered by clients, so only https is accepted.
        RuleFor(x => x.ImageUrl)
            .Must(BeAbsoluteHttpsUrl)
            .When(x => x.ImageUrl is not null)
            .WithMessage("The image URL must be an https address.");
    }

    /// <summary>
    /// Checks that the value is an absolute URL with the https scheme.
    /// </summary>
    /// <param name="imageUrl">The image URL being validated.</param>
    /// <returns><see langword="true"/> when the value is an absolute https URL.</returns>
    private static bool BeAbsoluteHttpsUrl(string? imageUrl)
    {
        return Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) && uri.Scheme == Uri.UriSchemeHttps;
    }
}
