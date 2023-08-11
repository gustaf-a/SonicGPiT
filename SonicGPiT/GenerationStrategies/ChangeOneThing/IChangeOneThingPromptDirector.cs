using Shared.Models;
using SonicGPiT.Models;

namespace SonicGPiT.GenerationStrategies.ChangeOneThing;

public interface IChangeOneThingPromptDirector
{
    public Prompt BuildPrompt(CodeRequest codeRequest);
}
