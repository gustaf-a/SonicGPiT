namespace Shared.Configurations;

public class FeatureFlagsOptions
{
    public const string FeatureFlags = "FeatureFlags";

    public bool UseExpensiveAiModel { get; set; } = false;
}
