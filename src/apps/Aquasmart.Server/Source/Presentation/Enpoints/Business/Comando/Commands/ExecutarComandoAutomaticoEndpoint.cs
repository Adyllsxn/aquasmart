namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.Comando.Commands;
public class ExecutarComandoAutomaticoEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/automatico", HandleAsync)
           .WithSummary("Executar comando automático (usado pelo sistema)")
           .WithName("ExecutarComandoAutomatico")
           .Produces<ComandoResponse>(201)
           .Produces(400)
           .Produces(500);

    private static async Task<IResult> HandleAsync(
        [FromServices] IComandoService service,
        [FromBody] ComandoRequest request,
        CancellationToken token)
    {
        var comando = await service.ExecutarComandoAutomaticoAsync(request.Tipo, request.Acao, token);
        
        var response = new ComandoResponse
        {
            Id = comando.Id,
            Tipo = comando.Tipo,
            Acao = comando.Acao,
            Timestamp = comando.Timestamp,
            ExecutadoPor = comando.ExecutadoPor,
            LeituraSensorId = comando.LeituraSensorId
        };
        
        return Results.Created($"/api/comandos/{comando.Id}", response);
    }
}