namespace Shared.Configuration;

public class CodeGenerationOptions
{
    public const string CodeGeneration = "CodeGeneration";

    public string Delimiter { get; set; } = "####";
    public string ExistingCodeTagText { get; set; } = "existing-code";
    public object UserInputTagText { get; set; } = "user-input";
}
