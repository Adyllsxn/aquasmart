namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.Alerta.Queries;
public class GetNaoLidosEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/nao-lidos", HandleAsync)
           .WithSummary("Obter alertas não lidos")
           .WithName("GetAlertasNaoLidos")
           .Produces<List<AlertaResponse>>(200)
           .Produces(500);

    private static async Task<IResult> HandleAsync(
        [FromServices] IAlertaService service,
        CancellationToken token)
    {
        var alertas = await service.GetNaoLidosAsync(token);
        
        var response = alertas.Select(a => new AlertaResponse
        {
            Id = a.Id,
            Mensagem = a.Mensagem,
            Tipo = a.Tipo,
            Gravidade = a.Gravidade,
            Timestamp = a.Timestamp,
            Lida = a.Lida,
            ValorRegistado = a.ValorRegistado,
            LeituraSensorId = a.LeituraSensorId
        }).ToList();
        
        return Results.Ok(response);
    }
}