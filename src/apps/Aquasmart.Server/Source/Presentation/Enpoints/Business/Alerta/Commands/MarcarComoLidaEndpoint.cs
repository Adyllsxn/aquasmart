namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.Alerta.Commands;
public class MarcarComoLidaEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut("/{id}/marcar-lida", HandleAsync)
           .WithSummary("Marcar um alerta como lido")
           .WithName("MarcarAlertaComoLido")
           .Produces(204)
           .Produces(404)
           .Produces(500);

    private static async Task<IResult> HandleAsync(
        [FromServices] IAlertaService service,
        [FromRoute] Guid id,
        CancellationToken token)
    {
        var alerta = await service.GetByIdAsync(id, token);
        
        if (alerta == null)
        {
            return Results.NotFound(new { error = $"Alerta com ID {id} não encontrado" });
        }
        
        await service.MarcarComoLidaAsync(id, token);
        
        return Results.NoContent();
    }
}