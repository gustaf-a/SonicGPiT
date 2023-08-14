namespace SonicGPiT.Models;

public class CodeRequest
{
    public string CurrentCode { get; set; }
    public string GenerationMethod { get; set; }
    public string UserInput { get; set; }
    public bool? UseExpensiveModel { get; set; }
}
