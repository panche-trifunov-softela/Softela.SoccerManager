namespace SoccerManager.Importer.Transform;

/// <summary>
/// Configuration for detecting placeholder player image URLs.
/// </summary>
public sealed class ImagesOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Transform:Images";

    /// <summary>The number of imported players an image URL must be shared by before it is treated as a placeholder and dropped.</summary>
    public int PlaceholderMinShare { get; set; } = 5;
}
