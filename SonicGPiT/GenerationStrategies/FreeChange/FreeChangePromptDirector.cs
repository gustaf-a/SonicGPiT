using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Prompts;
using SonicGPiT.Models;
using System.Text;

namespace SonicGPiT.GenerationStrategies.FreeChange;

public class FreeChangePromptDirector : IFreeChangePromptDirector
{
    private readonly IPromptBuilder _promptBuilder;
    private readonly CodeGenerationOptions _codeGenerationOptions;

    public FreeChangePromptDirector(IPromptBuilder promptBuilder, IOptions<CodeGenerationOptions> options)
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
Your task is to modify and create Sonic Pi code according to input from a user.
Use live loops where possible.

Perform these steps: 
1. Analyze the existing code to understand what feeling it has.
2. Analyze the input and create a list of suggestions on how to change the existing code.
3. Implement the changes.
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
