using GptHandler.Models;
using Shared.JsonHandler;

namespace GptHandler.GptClient;

public class GptResponseParser : IGptResponseParser
{
    private static readonly List<string> ValidFinishReasons = new() { "function_call", "stop" };

    public bool GetGptResponseFunctionCallName(string gptResponseString, string callerName, out string functionCallName)
    {
        functionCallName = string.Empty;

        var choice = ParseChoice(gptResponseString, callerName);
        if (choice is null)
            return false;

        if (string.IsNullOrWhiteSpace(choice.Message?.FunctionCall?.Name))
            return false;

        functionCallName = choice.Message.FunctionCall.Name;

        return true;
    }

    public T ParseGptResponseFunctionCall<T>(string gptResponseString, string callerName)
    {
        var choice = ParseChoice(gptResponseString, callerName);
        if (choice is null)
            return default;

        if (!ValidFinishReasons.Contains(choice.FinishReason))
            Console.WriteLine($"Possible corrupt finish response from GPT Client. Bad finish reason: {choice.FinishReason}.");

        var functionCall = choice.Message?.FunctionCall;
        if (functionCall is null)
        {
            Console.WriteLine($"Bad response from GPT Client. Function call not set in response. {callerName} failed.");
            return default;
        }

        var deserializedType = JsonHelper.Deserialize<T>(functionCall.Arguments);
        if (deserializedType is null)
        {
            Console.WriteLine($"Bad response from GPT Client. Unable to deserialize topic identification arguments. {callerName} failed.");
            return default;
        }

        return deserializedType;
    }

    public T ParseGptResponse<T>(string gptResponseString, string callerName)
    {
        var choice = ParseChoice(gptResponseString, callerName);
        if (choice is null)
            return default;

        var contentString = choice.Message.Content;
        if (contentString is null)
        {
            Console.WriteLine($"Bad response from GPT Client. Content not set in response. {callerName} failed.");
            return default;
        }

        var content = JsonHelper.Deserialize<T>(contentString);
        if (content is null)
        {
            Console.WriteLine($"Bad response from GPT Client. Unable to deserialize content. {callerName} failed.");
            return default;
        }

        return content;
    }

    private static GptResponseChoice ParseChoice(string gptResponseString, string callerName)
    {
        if (string.IsNullOrWhiteSpace(gptResponseString))
        {
            Console.WriteLine($"Failed to get response from GPT Client. Null response returned. {callerName} failed.");
            return default;
        }

        GptResponse gptResponse;

        try
        {
            gptResponse = JsonHelper.Deserialize<GptResponse>(gptResponseString);
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed deserialize response from GPT Client. {callerName} failed.");
            return default;
        }

        if (gptResponse is null)
        {
            Console.WriteLine($"Failed to get response from GPT Client. Failed to deserialize GptResponse. {callerName} failed.");
            return default;
        }

        var choice = gptResponse.Choices.FirstOrDefault();
        if (choice is null)
        {
            Console.WriteLine($"Failed to get response from GPT Client. No choices found. {callerName} failed.");
            return default;
        }

        return choice;
    }
}
