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

    public async Task<BackendResponse<CodeResponse>> GenerateRequest(CodeRequest codeRequest)
    {
        var backendResponse = new BackendResponse<CodeResponse>();

        var strategy = _codeGenerationStrategyFactory.Create(codeRequest);

        var codeResponse = await strategy.GenerateCode(codeRequest);

        //codeResponse.GeneratedCode = SonicPiCodeCleaner.CleanStringForSonicPiInput(codeResponse.GeneratedCode);        //codeResponse.GeneratedCode = SonicPiCodeCleaner.CleanStringForSonicPiInput(codeResponse.GeneratedCode);

        backendResponse.IsSuccess = true;
        backendResponse.Data = codeResponse;

        return backendResponse;
    }
}
