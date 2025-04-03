using Microsoft.Extensions.DependencyInjection;
using OCR.AIVision.Application.Interface.Service;
using OCR.AIVision.Application.Service;

namespace OCR.AIVision.Application.Configuration.Injections;

public static class ServiceConfig
{
    public static IServiceCollection AddServices(this IServiceCollection service) =>
        service.AddScoped<IOCRService, OCRService>();
}
