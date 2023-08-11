using SonicGPiT.GenerationStrategies;
using SonicGPiT.Models;
using SonicGPiT.Utils;

namespace SonicGPiT.Services;

public class CodeGeneratorService : ICodeGeneratorService
{
    private readonly ICodeGenerationStrategyFactory _codeGenerationStrategyFactory;

    public CodeGeneratorService(ICodeGenerationStrategyFactory codeGenerationStrategyFactory)
    {
        _codeGenerationStrategyFactory = codeGenerationStrategyFactory;
    }

    public async Task<CodeResponse> GenerateRequest(CodeRequest codeRequest)
    {
        var strategy = _codeGenerationStrategyFactory.Create(codeRequest);

        var response = await strategy.GenerateCode(codeRequest);
        
        response.GeneratedCode = SonicPiCodeCleaner.CleanStringForSonicPiInput(response.GeneratedCode);

        return response;
    }
}
