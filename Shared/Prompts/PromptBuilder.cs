using Microsoft.Extensions.Options;
using Shared.Configurations;
using Shared.Extensions;
using Shared.JsonHandler;
using Shared.Models;

namespace Shared.Prompts;

public class PromptBuilder : IPromptBuilder
{
    private readonly OpenAiOptions _openAiOptions;
    private readonly FeatureFlagsOptions _featureFlagOptions;
    private readonly List<PromptMessage> _promptMessages = new();
    private readonly List<PromptFunction> _promptFunctions = new();

    public PromptBuilder(IOptions<OpenAiOptions> openAiOptions, IOptions<FeatureFlagsOptions> featureFlagsOptions)
    {
        _openAiOptions = openAiOptions.Value;

        if(_openAiOptions is null)
            throw new ArgumentNullException(nameof(openAiOptions));

        _featureFlagOptions = featureFlagsOptions.Value;
    }

    public void Reset()
    {
        _promptFunctions.Clear();
        _promptMessages.Clear();  
    }

    public Prompt GetPrompt(bool? useExpensiveModel = null)
    {
        if (_promptMessages.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(_promptMessages), $"Invalid prompt: systemPrompt and userPrompt both empty");

        var prompt = new Prompt
        {
            Model = GetModel(useExpensiveModel),
            Messages = _promptMessages,
            Functions = _promptFunctions,
            FunctionCall = GetFunctionCall()
        };

        return prompt;
    }

    public string GetModel(bool? useExpensiveModel)
    {
        if(useExpensiveModel ?? _featureFlagOptions.UseExpensiveAiModel)
        {
            if (string.IsNullOrWhiteSpace(_openAiOptions.ApiModelExpensive))
                throw new ArgumentNullException(nameof(_openAiOptions.ApiModelExpensive));

            return _openAiOptions.ApiModelExpensive;
        }

        if (string.IsNullOrWhiteSpace(_openAiOptions.ApiModelBase))
            throw new ArgumentNullException(nameof(_openAiOptions.ApiModelBase));

        return _openAiOptions.ApiModelBase;
    }

    public void AddFunctionCalls(List<PromptFunction> promptFunctions)
    {
        if (promptFunctions.IsNullOrEmpty())
            return;

        foreach (var promptFunction in promptFunctions)
            AddFunctionCall(promptFunction);
    }

    public void AddFunctionCall(string promptFunctionJson)
    {
        if(string.IsNullOrWhiteSpace(promptFunctionJson))
            throw new ArgumentNullException(nameof(promptFunctionJson));

        var functionCall = JsonHelper.TryDeserialize<PromptFunction>(promptFunctionJson);
        if (functionCall != null)
        {
            AddFunctionCall(functionCall);
            return;
        }

        var functionCallList = JsonHelper.TryDeserialize<List<PromptFunction>>(promptFunctionJson);
        if (functionCallList.IsNullOrEmpty())
            throw new ArgumentException(nameof(promptFunctionJson), "Failed to deserialize input into PromptFunction or List<PromptFunction>");

        foreach (var call in functionCallList)
            AddFunctionCall(call);

        return;
    }

    public void AddFunctionCall(PromptFunction promptFunction)
    {
        if (promptFunction is null)
            return;

        _promptFunctions.Add(promptFunction);
    }

    public void AddSystemMessage(string systemPrompt)
    {
        AddPromptMessage(systemPrompt, PromptRoles.System);
    }

    public void AddUserMessage(string userPrompt)
    {
        AddPromptMessage(userPrompt, PromptRoles.User);
    }

    private void AddPromptMessage(string prompt, string role)
    {
        _promptMessages.Add(new PromptMessage
        {
            Role = role,
            Content = prompt
        });
    }

    private PromptFunctionCall GetFunctionCall()
    {
        var promptFunctionCall = "none";

        if (!_promptFunctions.IsNullOrEmpty())
            promptFunctionCall = _promptFunctions.Count == 1 ? _promptFunctions[0].Name : "auto";

        return new PromptFunctionCall
        {
            Name = promptFunctionCall
        };
    }
}
