using OCR.AIVision.Application.Helpers;
using OCR.AIVision.Application.Interface.Service;
using OCR.AIVision.Domain.Entities.Request;
using OCR.AIVision.Domain.Entities.Response;
using System.Text;

namespace OCR.AIVision.Api.Endpoints;

public static class OCREndpoints
{
    public static WebApplication AddOCREndpoints(this WebApplication app)
    {
        var group = app.MapGroup("OCR");

        group.MapPost("/ProcessarImagemDeArquivo", async (HttpRequest request, string correlationId, IOCRService _service) =>
        {
            try
            {
                if (!request.HasFormContentType || request.Form.Files.Count == 0)
                    return Results.BadRequest(new OCRResponse {Excecao = "Nenhuma imagem foi enviada.", CorrelationId = correlationId });

                var imagemParaAnalisar = request.Form.Files[0];
                var response = await _service.ProcessarImagem(imagemParaAnalisar, correlationId);

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(string.Concat("Não foi possível analisar a imagem, request id:", correlationId, ", Erro: ", Helper.GetErrorMessage(ex)));
            }
        })
        .WithOpenApi()
        .WithTags("OCR");

        group.MapPost("/ProcessarImagemDeStringBase64", async (OCRRequest request, IOCRService _service) =>
        {
            try
            {
                if (string.IsNullOrEmpty(request.ImagemBase64))
                    return Results.BadRequest(new OCRResponse { Excecao = "Nenhuma imagem foi enviada.", CorrelationId = request.CorrelationId});

                var response = await _service.ProcessarImagem(request.ImagemBase64, request.CorrelationId);

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(string.Concat("Não foi possível analisar a imagem, request id:", request.CorrelationId, " Erro: ", Helper.GetErrorMessage(ex)));
            }
        })
        .WithOpenApi()
        .WithTags("OCR");

        return app;
    }
}