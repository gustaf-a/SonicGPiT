using Newtonsoft.Json;

namespace Shared.JsonHandler;

public static class JsonHelper
{
    public static async Task<T> GetObjectFromJson<T>(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        try
        {
            string fileAsString = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrWhiteSpace(fileAsString))
                return default;

            var obj = Deserialize<T>(fileAsString);

            return obj ?? default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return default;
        }
    }

    public static async Task SaveToJson(object objectToSave, string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        var claimClaimChecksJson = Serialize(objectToSave);

        await File.WriteAllTextAsync(filePath, claimClaimChecksJson);
    }

    public static string Serialize(object ojectToSerialize, bool includeNullValues = true)
    {
        JsonSerializerSettings settings = new()
        {
            NullValueHandling = includeNullValues ? NullValueHandling.Include : NullValueHandling.Ignore
        };

        return JsonConvert.SerializeObject(ojectToSerialize, settings);
    }

    public static T Deserialize<T>(string jsonString)
    {
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    public static T TryDeserialize<T>(string promptFunctionJson)
    {
        try
        {
            return Deserialize<T>(promptFunctionJson);
        }
        catch (Exception)
        {
            return default;
        }
    }
}
