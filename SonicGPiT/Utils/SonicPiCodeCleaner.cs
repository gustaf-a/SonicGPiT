using Shared.Extensions;

namespace SonicGPiT.Utils;

public static class SonicPiCodeCleaner
{
    public static string CleanStringForSonicPiInput(string input)
    {
        var fixedLineEndings = input.ConvertToWindowsLineEndings();

        return fixedLineEndings;
    }
}
