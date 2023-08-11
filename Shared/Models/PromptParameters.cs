using Newtonsoft.Json;

namespace Shared.Models;

public class PromptParameters
{
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; } = "object";

    [JsonProperty(PropertyName = "properties")]
    public object Properties { get; set; }

    [JsonProperty(PropertyName = "required")]
    public List<string> Required { get; set; }
}