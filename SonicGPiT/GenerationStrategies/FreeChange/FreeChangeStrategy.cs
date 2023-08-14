using GptHandler.GptClient;
using SonicGPiT.GenerationStrategies.ChangeOneThing;
using SonicGPiT.Models;

namespace SonicGPiT.GenerationStrategies.FreeChange;

public class FreeChangeStrategy : ICodeGenerationStrategy
{
    private readonly IFreeChangePromptDirector _promptDirector;
    private readonly IGptClient _gptClient;
    private readonly IGptResponseParser _gptResponseParser;

    public string Name => nameof(FreeChangeStrategy);

    public FreeChangeStrategy(IFreeChangePromptDirector freeChangePromptDirector, IGptClient gptClient, IGptResponseParser gptResponseParser)
    {
        _promptDirector = freeChangePromptDirector;
        _gptClient = gptClient;
        _gptResponseParser = gptResponseParser;
    }

    public async Task<CodeResponse> GenerateCode(CodeRequest request)
    {
        var response = new CodeResponse();

        var prompt = _promptDirector.BuildPrompt(request);

        try
        {
            var gptResponse = await _gptClient.GetCompletion(prompt);
            if (gptResponse is null)
            {
                response.ErrorMessages.Add($"Failed to get response from GPT.");
                return response;
            }

            var gptCodeGeneratedResponse = _gptResponseParser.ParseGptResponseFunctionCall<GptCodeGeneratedResponse>(gptResponse, nameof(ChangeOneThingStrategy));
            if (gptCodeGeneratedResponse is null)
            {
                response.ErrorMessages.Add($"Failed to parse response from GPT.");
                return response;
            }

            if (string.IsNullOrWhiteSpace(gptCodeGeneratedResponse.Code))
            {
                response.ErrorMessages.Add($"Code returned from GPT was nothing.");
                return response;
            }

            response.GeneratedCode = gptCodeGeneratedResponse.Code;
            response.IsSuccess = true;

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected exception encountered when trying to get response from GPT client. Unable to deserialize claims with references. {nameof(ChangeOneThingStrategy)} failed.", ex);
            response.ErrorMessages.Add($"Failed to get response from GPT: {ex.Message}");
            return response;
        }
    }
}
