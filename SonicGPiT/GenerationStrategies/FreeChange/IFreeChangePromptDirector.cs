using Shared.Models;
using SonicGPiT.Models;

namespace SonicGPiT.GenerationStrategies.FreeChange;

public interface IFreeChangePromptDirector
{
    public Prompt BuildPrompt(CodeRequest codeRequest);
}
