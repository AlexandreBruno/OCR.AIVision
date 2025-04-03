
using OCR.AIVision.Api.Endpoints;
using OCR.AIVision.Application.Configuration.Injections;
using Oci.Common;

namespace OCR.AIVision.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();
        ServiceConfig.AddServices(builder.Services);
        
        var app = builder.Build();
        app.UseHealthChecks(builder.Configuration["HealthEndpoint"]);
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.AddOCREndpoints();
        app.Run();
    }
}
