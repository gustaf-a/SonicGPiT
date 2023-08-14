using SonicGPiT.GenerationStrategies.FreeChange;
using SonicGPiT.Models;

namespace SonicGPiT.GenerationStrategies;

public class CodeGenerationStrategyFactory : ICodeGenerationStrategyFactory
{
    private const string DefaultStrategyName = nameof(FreeChangeStrategy);

    private readonly IEnumerable<ICodeGenerationStrategy> _codeGenerationStrategies;

    public CodeGenerationStrategyFactory(IEnumerable<ICodeGenerationStrategy> codeGenerationStrategies)
    {
        _codeGenerationStrategies = codeGenerationStrategies;
    }

    public ICodeGenerationStrategy Create(CodeRequest codeRequest)
    {
        var selectedStrategyName = codeRequest.GenerationMethod ?? "";

        if (!string.IsNullOrWhiteSpace(selectedStrategyName))
            foreach (var strategy in _codeGenerationStrategies)
                if (selectedStrategyName.Equals(strategy.Name))
                    return strategy;

        Console.WriteLine($"Failed to find code generation strategy corresponding to: {codeRequest.GenerationMethod}. Returning default strategy.");

        var defaultStrategy = _codeGenerationStrategies.Where(s => DefaultStrategyName.Equals(s.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

        return defaultStrategy 
            ?? throw new Exception($"Failed to find default strategy corresponding to: {DefaultStrategyName}");
    }
}
