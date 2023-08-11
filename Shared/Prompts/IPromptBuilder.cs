using Shared.Models;

namespace Shared.Prompts;

public interface IPromptBuilder
{
    public Prompt GetPrompt();

    public void AddModel(string model);
    public string GetModel();

    public void AddSystemMessage(string systemPrompt);
    public void AddUserMessage(string userPrompt);

    public void AddFunctionCall(string promptFunctionJson);
    public void AddFunctionCall(PromptFunction promptFunction);
    public void AddFunctionCalls(List<PromptFunction> promptFunctions);
    public void Reset();
}
