namespace OCR.AIVision.Domain.Entities.Request;

public class OCRRequest
{
    public string ImagemBase64 { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
}