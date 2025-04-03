using OCR.AIVision.Domain.Entities.Response;
using Microsoft.AspNetCore.Http;

namespace OCR.AIVision.Application.Interface.Service
{
    public interface IOCRService
    {
        Task<OCRResponse> ProcessarImagem(IFormFile imagemParaAnalisar, string correlationId);
        Task<OCRResponse> ProcessarImagem(string imagemParaAnalisarEmBase64, string correlationId);
    }
}
