namespace SonicGPiT.Models;

public class CodeResponse
{
    public bool IsSuccess { get; set; }
    public List<string> ErrorMessages { get; set; } = new();

    public string GeneratedCode { get; set; }
}
