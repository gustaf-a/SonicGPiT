namespace Shared.Extensions;

public static class StringExtensions
{
    public static string ConvertToWindowsLineEndings(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return input.Replace("\r\n", "\n").Replace("\n", "\r\n");
    }
}
