namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.Comando.Queries;
public class GetComandosRecentesEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/recentes", HandleAsync)
           .WithSummary("Obter comandos recentes dos últimos X minutos")
           .WithName("GetComandosRecentes")
           .Produces<List<ComandoResponse>>(200)
           .Produces(400)
           .Produces(500);

    private static async Task<IResult> HandleAsync(
        [FromServices] IComandoService service,
        CancellationToken token,
        [FromQuery] int minutos = 60)
    {
        if (minutos <= 0 || minutos > 1440)
        {
            return Results.BadRequest(new { error = "Os minutos devem ser entre 1 e 1440" });
        }
        
        var comandos = await service.GetComandosRecentesAsync(minutos, token);
        
        var response = comandos.Select(c => new ComandoResponse
        {
            Id = c.Id,
            Tipo = c.Tipo,
            Acao = c.Acao,
            Timestamp = c.Timestamp,
            ExecutadoPor = c.ExecutadoPor,
            LeituraSensorId = c.LeituraSensorId
        }).ToList();
        
        return Results.Ok(response);
    }
}