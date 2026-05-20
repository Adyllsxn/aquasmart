namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.Comando.Commands;
public class ExecutarComandoManualEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/manual", HandleAsync)
           .WithSummary("Executar comando manual (bomba, alimentador, modo)")
           .WithName("ExecutarComandoManual")
           .Produces<ComandoResponse>(201)
           .Produces(400)
           .Produces(500);

    private static async Task<IResult> HandleAsync(
        [FromServices] IComandoService service,
        [FromBody] ComandoRequest request,
        CancellationToken token)
    {
        var comando = await service.ExecutarComandoManualAsync(request.Tipo, request.Acao, token);
        
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