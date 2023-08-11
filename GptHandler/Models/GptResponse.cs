using Newtonsoft.Json;

namespace GptHandler.Models;

public class GptResponse
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "object")]
    public string Object { get; set; }

    [JsonProperty(PropertyName = "created")]
    public long Created { get; set; }

    [JsonProperty(PropertyName = "model")]
    public string Model { get; set; }

    [JsonProperty(PropertyName = "choices")]
    public List<GptResponseChoice> Choices { get; set; }

    [JsonProperty(PropertyName = "usage")]
    public GptResponseUsage Usage { get; set; }
}
