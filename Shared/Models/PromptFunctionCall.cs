using Newtonsoft.Json;

namespace Shared.Models;

public class PromptFunctionCall
{
    /// <summary>
    /// Name of the function to call
    /// </summary>
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = "auto";
}
