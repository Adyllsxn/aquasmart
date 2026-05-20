namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.LeituraSensor.Queries;
public class GetHistoricoEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/historico", HandleAsync)
           .WithSummary("Obter histórico de leituras dos últimos X minutos")
           .WithName("GetHistoricoLeituras")
           .Produces<HistoricoResponse>(200)
           .Produces(400)
           .Produces(500);

    private static async Task<IResult> HandleAsync(
        [FromServices] ILeituraSensorService service,
        CancellationToken token,
        [FromQuery] int minutos = 60)
    {
        if (minutos <= 0 || minutos > 1440)
        {
            return Results.BadRequest(new { error = "Os minutos devem ser entre 1 e 1440" });
        }
        
        var leituras = await service.GetHistoricoAsync(minutos, token);
        
        var response = new HistoricoResponse
        {
            PeriodoMinutos = minutos,
            TotalLeituras = leituras.Count,
            Leituras = leituras.Select(l => new LeituraSensorResponse
            {
                Id = l.Id,
                Timestamp = l.Timestamp,
                Temperatura = l.Temperatura,
                Ph = l.Ph,
                Oxigenio = l.Oxigenio,
                NivelAgua = l.NivelAgua
            }).ToList()
        };
        
        return Results.Ok(response);
    }
}