using Shared.Models;

namespace Shared.Prompts;

public interface IPrompt
{
    public Prompt GetPrompt(string systemMessage, string userMessage, List<PromptFunction> functions, string model);
}
