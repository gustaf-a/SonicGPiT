using SonicGPiT.Models;

namespace SonicGPiT.Services;

public interface ICodeGeneratorService
{
    public Task<CodeResponse> GenerateRequest(CodeRequest codeRequest);
}
