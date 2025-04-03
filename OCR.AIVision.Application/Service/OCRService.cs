using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oci.AivisionService;
using Oci.AivisionService.Models;
using Oci.AivisionService.Requests;
using Oci.Common;
using Oci.Common.Auth;
using OCR.AIVision.Application.Helpers;
using OCR.AIVision.Application.Interface.Service;
using OCR.AIVision.Domain.Entities.Response;

namespace OCR.AIVision.Application.Service;

public class OCRService : IOCRService
{
    private readonly IConfiguration _configuration;
    private readonly string _compartmentId;
    private readonly string _configFile;
    private readonly ILogger<OCRService> _logger;

    public OCRService(IConfiguration configuration, ILogger<OCRService> logger)
    {
        _configuration = configuration;
        _compartmentId = _configuration["OracleSettings:CompartmentId"]!;
        _configFile = Path.Combine(AppContext.BaseDirectory, "oci.config");
        _logger = logger;
    }
    public async Task<OCRResponse> ProcessarImagem(IFormFile imagemParaAnalisar, string correlationId)
    {
        _logger.LogInformation("Iniciando processamento de arquivo da request id: {correlationId}", correlationId);
        var retorno = new OCRResponse { CorrelationId = correlationId };

        var tipoDeImagemPermitido = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        var extensaoDoArquivo = Path.GetExtension(imagemParaAnalisar.FileName).ToLower();
        if (!tipoDeImagemPermitido.Contains(extensaoDoArquivo))
        {
            _logger.LogError("Arquivo inválido, extensão {extensaoDoArquivo} request id:{correlationId}", extensaoDoArquivo , correlationId);
            retorno.Mensagem = "Arquivo inválido, extensões aceitas: jpg, jpeg, png, bmp e gif";
            return retorno;
        }

        byte[] imagemEmBytes;
        using (var memoryStream = new MemoryStream())
        {
            await imagemParaAnalisar.CopyToAsync(memoryStream);
            imagemEmBytes = memoryStream.ToArray();
        }

        retorno = await RequestOCI(imagemEmBytes, retorno);

        return retorno;
    }
    public async Task<OCRResponse> ProcessarImagem(string imagemParaAnalisarEmBase64, string correlationId)
    {
        _logger.LogInformation("Iniciando processamento de string base64 da request id:{correlationId}", correlationId);
        var retorno = new OCRResponse { CorrelationId = correlationId };
        byte[] imagemEmBytes;
        try
        {
            imagemEmBytes = Convert.FromBase64String(imagemParaAnalisarEmBase64);
        }
        catch (Exception ex)
        {
            var erro = Helper.GetErrorMessage(ex);
            _logger.LogError("Não foi possivel transformar a string em bytes, request id:{correlationId} Erro: {erro}", correlationId, erro);
            retorno.Mensagem = "Não foi possivel transformar a string em bytes!";
            retorno.Excecao = erro;
            return retorno;
        }

        retorno = await RequestOCI(imagemEmBytes, retorno);

        return retorno;
    }
    private async Task<OCRResponse> RequestOCI(byte[] imagemEmBytes, OCRResponse response)
    {
        try
        {
            var parametrosParaAnaliseDaImagem = new AnalyzeImageDetails
            {
                Features =
                [
                    new ImageTextDetectionFeature{ }/*,//Utilizo apenas o reconhecimento de texto
                    new ImageClassificationFeature{ },//Não utilizo classificação do que existe na imagem
                    new FaceDetectionFeature{ },*///Não utilizo a identificação de rostos
                ],
                Image = new InlineImageDetails
                {
                    Data = imagemEmBytes
                },
                CompartmentId = _compartmentId
            };

            var analyzeImageRequest = new AnalyzeImageRequest
            {
                AnalyzeImageDetails = parametrosParaAnaliseDaImagem,
                OpcRequestId = Guid.NewGuid().ToString()
            };
            var provider = new ConfigFileAuthenticationDetailsProvider(_configFile, "DEFAULT");

            using var client = new AIServiceVisionClient(provider, new ClientConfiguration());
            var responseOCI = await client.AnalyzeImage(analyzeImageRequest);

            var listaTextoExtraido = responseOCI?.AnalyzeImageResult?.ImageText?.Lines;
            if (listaTextoExtraido != null && listaTextoExtraido.Count != 0)
            {
                foreach (var linha in listaTextoExtraido)
                    response.TextoExtraido.Add(linha.Text);

                response.ImagemProcessadaComSucesso = true;
            }
            else
                response.Mensagem = "Imagem processada, porém nenhum texto foi identificado.";
        }
        catch (Exception ex)
        {
            var erro = Helper.GetErrorMessage(ex);
            _logger.LogError("Não foi possivel processar a imagem enviada, request id:{CorrelationId} Erro: {erro}", response.CorrelationId, erro);
            _logger.LogError("request id:{CorrelationId} Bytes: {imagemEmBytes}", response.CorrelationId, imagemEmBytes);
            response.Excecao = erro;
            response.Mensagem = "Não foi possível processar a imagem!";
        }

        return response;
    }
}
