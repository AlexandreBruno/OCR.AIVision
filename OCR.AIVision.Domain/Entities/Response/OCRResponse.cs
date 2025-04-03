namespace OCR.AIVision.Domain.Entities.Response;
public class OCRResponse
{
    public bool ImagemProcessadaComSucesso { get; set; } = false;
    public List<string> TextoExtraido { get; set; } = new List<string>();
    public string Mensagem { get; set; } = string.Empty;
    public string Excecao { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;
}