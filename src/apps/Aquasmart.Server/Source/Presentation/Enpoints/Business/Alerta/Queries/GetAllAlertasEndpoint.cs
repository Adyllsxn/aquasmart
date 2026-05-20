namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.Alerta.Queries;
public class GetAllAlertasEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
           .WithSummary("Obter todos os alertas")
           .WithName("GetAllAlertas")
           .Produces<List<AlertaResponse>>(200)
           .Produces(500);

    private static async Task<IResult> HandleAsync(
        [FromServices] IAlertaService service,
        CancellationToken token)
    {
        var alertas = await service.GetAllAsync(token);
        
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