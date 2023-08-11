using SonicGPiT.Models;

namespace SonicGPiT.GenerationStrategies;

public interface ICodeGenerationStrategy
{
    public string Name { get; }

    public Task<CodeResponse> GenerateCode(CodeRequest request);
}
