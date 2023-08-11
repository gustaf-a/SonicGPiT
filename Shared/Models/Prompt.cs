using Newtonsoft.Json;

namespace Shared.Models;

public class Prompt
{
    [JsonProperty(PropertyName = "model")]
    public string? Model { get; set; }
    
    [JsonProperty(PropertyName = "messages")]
    public List<PromptMessage> Messages { get; set; } = new();

    [JsonProperty(PropertyName = "functions")]
    public List<PromptFunction> Functions { get; set; } = new();

    [JsonProperty(PropertyName = "function_call")]
    public PromptFunctionCall? FunctionCall { get; set; }
}
