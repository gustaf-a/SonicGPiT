using Newtonsoft.Json;

namespace GptHandler.Models;

public class GptResponseChoice
{
    [JsonProperty(PropertyName = "index")]
    public int Index { get; set; }

    [JsonProperty(PropertyName = "message")]
    public GptResponseMessage Message { get; set; }

    [JsonProperty(PropertyName = "finish_reason")]
    public string FinishReason { get; set; }
}
