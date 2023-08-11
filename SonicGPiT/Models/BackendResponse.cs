using Newtonsoft.Json;

namespace SonicGPiT.Models;

public class BackendResponse<T>
{
    [JsonProperty(PropertyName = "data")]
    public T? Data { get; set; }

    [JsonProperty(PropertyName = "messages")]
    public List<string> Messages { get; set; } = new();

    [JsonProperty(PropertyName = "isSuccess")]
    public bool IsSuccess { get; set; } = false;
}
