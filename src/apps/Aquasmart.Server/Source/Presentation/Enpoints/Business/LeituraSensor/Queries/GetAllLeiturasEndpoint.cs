namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.LeituraSensor.Queries;
public class GetAllLeiturasEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
           .WithSummary("Obter todas as leituras dos sensores")
           .WithName("GetAllLeituras")
           .Produces<List<LeituraSensorResponse>>(200)
           .Produces(500);

    private static async Task<IResult> HandleAsync(
        [FromServices] ILeituraSensorService service,
        CancellationToken token)
    {
        var leituras = await service.GetAllAsync(token);
        
        var response = leituras.Select(l => new LeituraSensorResponse
        {
            Id = l.Id,
            Timestamp = l.Timestamp,
            Temperatura = l.Temperatura,
            Ph = l.Ph,
            Oxigenio = l.Oxigenio,
            NivelAgua = l.NivelAgua
        }).ToList();
        
        return Results.Ok(response);
    }
}