using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Prompts;
using SonicGPiT.Models;
using System.Text;

namespace SonicGPiT.GenerationStrategies.ChangeOneThing;

public class ChangeOneThingPromptDirector : IChangeOneThingPromptDirector
{
    private readonly IPromptBuilder _promptBuilder;
    private readonly CodeGenerationOptions _codeGenerationOptions;

    public ChangeOneThingPromptDirector(IPromptBuilder promptBuilder, IOptions<CodeGenerationOptions> options)
    {
        _promptBuilder = promptBuilder;
        _codeGenerationOptions = options.Value;
    }

    public Prompt BuildPrompt(CodeRequest codeRequest)
    {
        _promptBuilder.Reset();

        _promptBuilder.AddFunctionCall(Functions);

        _promptBuilder.AddSystemMessage(GetSystemMessage(codeRequest));

        _promptBuilder.AddUserMessage(GetUserMessage(codeRequest));

        return _promptBuilder.GetPrompt();
    }

    private string GetSystemMessage(CodeRequest codeRequest)
    {
        var sb = new StringBuilder();

        sb.Append(SystemPrompt);

        sb.AppendLine($"<{_codeGenerationOptions.ExistingCodeTagText}>");
        sb.AppendLine(codeRequest.CurrentCode);
        sb.AppendLine($"</{_codeGenerationOptions.ExistingCodeTagText}>");

        return sb.ToString();
    }

    private string GetUserMessage(CodeRequest codeRequest)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<{_codeGenerationOptions.UserInputTagText}>");
        sb.AppendLine(codeRequest.UserInput);
        sb.AppendLine($"</{_codeGenerationOptions.UserInputTagText}>");

        return sb.ToString().Trim();
    }

    private string SystemPrompt { get; set; } =
"""
Your task is to modify Sonic Pi code according to input from a user.
Only modify one part of the existing code. 

Perform these steps: 
1. Analyze the existing code to understand what feeling it has.
2. Analyze the input and create a list of suggestions on how to change the existing code.
3. List possible parts of the existing code that could be changed.
4. Identify one single part to change and create the new code.
""";

    private string Functions { get; set; } =
"""
[
  {
    "name": "send_code",
    "description": "Sends Sonic Pi code that has been changed according to user input.",
    "parameters": {
      "type": "object",
      "properties": {
        "code": {
        "type": "string",
        "description": "The generated Sonic Pi code."
        },
      },
      "required": [ "code" ]
    }
  }
]
""";
}
