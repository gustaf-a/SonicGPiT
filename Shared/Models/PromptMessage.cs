using Newtonsoft.Json;

namespace Shared.Models;

public class PromptMessage
{
    [JsonProperty(PropertyName = "role")]
    public string Role { get; set; }

    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }
}
