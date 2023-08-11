using SonicGPiT.Models;

namespace SonicGPiT.GenerationStrategies;

public class CodeGenerationStrategyFactory : ICodeGenerationStrategyFactory
{
    private readonly IEnumerable<ICodeGenerationStrategy> _codeGenerationStrategies;

    public CodeGenerationStrategyFactory(IEnumerable<ICodeGenerationStrategy> codeGenerationStrategies)
    {
        _codeGenerationStrategies = codeGenerationStrategies;
    }

    public ICodeGenerationStrategy Create(CodeRequest codeRequest)
    {
        return _codeGenerationStrategies.First();
    }
}
