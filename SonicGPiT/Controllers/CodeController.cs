using Microsoft.AspNetCore.Mvc;
using SonicGPiT.Models;
using SonicGPiT.Services;

namespace SonicGPiT.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/code")]
public class CodeController : ControllerBase
{
    private readonly ICodeGeneratorService _codeGeneratorService;

    public CodeController(ICodeGeneratorService codeGeneratorService)
    {
        _codeGeneratorService = codeGeneratorService;
    }

    [HttpPost]
    public async Task<CodeResponse> GenerateCodeRequest([FromBody] CodeRequest codeRequest)
    {
        return await _codeGeneratorService.GenerateRequest(codeRequest);
    }

    [HttpGet("strategies")]
    public async Task<IEnumerable<string>> GetAllCodeGenerationStrategies()
    {
        return new List<string> { "ChangeOneThing" };
    }
}