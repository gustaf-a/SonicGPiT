namespace Shared.Configurations;

public class OpenAiOptions
{
    public const string OpenAi = "OpenAi";

    public string ApiKey { get; set; } = string.Empty;
    public string ApiModelBase { get; set; } = "gpt-3.5-turbo";
    public string ApiModelExpensive { get; set; } = "gpt-4";
    public int MaxTokens { get; set; } = 4000;

    public bool LogPrompts { get; set; } = true;
    public bool LogResponses { get; set; } = true;

    public string CompletionsUrl { get; set; } = @"https://api.openai.com/v1/chat/completions";
}
