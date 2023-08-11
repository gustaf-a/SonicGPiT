using SonicGPiT.Models;

namespace SonicGPiT.Services;

public interface ICodeGeneratorService
{
    public Task<BackendResponse<CodeResponse>> GenerateRequest(CodeRequest codeRequest);
}
