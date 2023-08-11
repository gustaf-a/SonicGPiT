using Shared.Models;

namespace GptHandler.GptClient;

public interface IGptClient
{
    public Task<string> GetCompletion(Prompt prompt, double temperature = 0);
    public Task<string> GetCompletion(string prompt, double temperature = 0);
}
