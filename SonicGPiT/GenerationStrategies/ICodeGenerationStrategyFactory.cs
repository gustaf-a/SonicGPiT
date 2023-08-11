namespace SonicGPiT.GenerationStrategies;

public interface ICodeGenerationStrategyFactory
{
    public ICodeGenerationStrategy Create(Models.CodeRequest codeRequest);
}
