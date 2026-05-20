namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.LeituraSensor.Queries;
public class GetUltimaLeituraEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/ultima", HandleAsync)
           .WithSummary("Obter a última leitura dos sensores")
           .WithName("GetUltimaLeitura")
           .Produces<LeituraSensorResponse>(200)
           .Produces(404)
           .Produces(500);

    private static async Task<IResult> HandleAsync(
        [FromServices] ILeituraSensorService service,
        CancellationToken token)
    {
        var leitura = await service.GetUltimaLeituraAsync(token);
        
        if (leitura == null)
        {
            return Results.NotFound(new { error = "Nenhuma leitura encontrada" });
        }
        
        var response = new LeituraSensorResponse
        {
            Id = leitura.Id,
            Timestamp = leitura.Timestamp,
            Temperatura = leitura.Temperatura,
            Ph = leitura.Ph,
            Oxigenio = leitura.Oxigenio,
            NivelAgua = leitura.NivelAgua
        };
        
        return Results.Ok(response);
    }
}