using Newtonsoft.Json;

namespace Shared.Models;

public class PromptFunction
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    [JsonProperty(PropertyName = "parameters")]
    public PromptParameters Parameters { get; set; }
}
