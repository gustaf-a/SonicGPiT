using Microsoft.Extensions.Options;
using Shared.Configurations;
using Shared.JsonHandler;
using Shared.Models;
using System.Net.Http.Headers;
using System.Text;

namespace GptHandler.GptClient;

public class GptClient : IGptClient
{
    private readonly HttpClient _httpClient;
    private readonly OpenAiOptions _openAiOptions;

    public GptClient(IHttpClientFactory clientFactory, IOptions<OpenAiOptions> openAiOptions)
    {
        _httpClient = clientFactory.CreateClient();
        _openAiOptions = openAiOptions.Value;

        if (string.IsNullOrWhiteSpace(_openAiOptions.ApiKey))
            throw new ArgumentNullException(nameof(_openAiOptions.ApiKey));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _openAiOptions.ApiKey);
    }

    /// <summary>
    /// Serializes a Prompt-object and sends it to GPT
    /// </summary>
    public async Task<string> GetCompletion(Prompt prompt, double temperature = 0)
    {
        var promptString = JsonHelper.Serialize(prompt, false);

        return await GetCompletion(promptString, temperature);
    }

    /// <summary>
    /// Sends a string containing a JSON formatted prompt to GPT
    /// </summary>
    public async Task<string> GetCompletion(string prompt, double temperature = 0)
    {
        if (_openAiOptions.LogPrompts)
            Log(nameof(GptClient) + " Prompt: " + prompt);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_openAiOptions.CompletionsUrl),
            Content = new StringContent(prompt, Encoding.UTF8, "application/json")
        };

        return await SendPostRequest(request);
    }

    private async Task<string> SendPostRequest(HttpRequestMessage request)
    {
        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(nameof(GptClient) + $": Request failed with status code {response.StatusCode}");

            if (_openAiOptions.LogResponses)
                Log(nameof(GptClient) + " Response: " + responseContent);

            return string.Empty;
        }

        Console.WriteLine(nameof(GptClient) + $": Request success.");

        if (_openAiOptions.LogResponses)
            Log(nameof(GptClient) + " Response: " + responseContent);

        return responseContent;
    }

    private static void Log(string msg)
    {
        Console.WriteLine(msg);
    }
}
