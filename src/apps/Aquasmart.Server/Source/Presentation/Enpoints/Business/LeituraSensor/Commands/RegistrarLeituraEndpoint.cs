namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.LeituraSensor.Commands;
public class RegistrarLeituraEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/api/leituras", HandleAsync)
           .WithSummary("Registrar nova leitura dos sensores")
           .WithName("RegistrarLeitura");

    private static async Task<IResult> HandleAsync(
        [FromServices] ILeituraSensorService service,
        [FromBody] RegistrarLeituraRequest request,
        CancellationToken token)
    {
        var leitura = new LeituraSensorEntity(
            request.Temperatura,
            request.Ph,
            request.Oxigenio,
            request.NivelAgua);

        var result = await service.RegistrarLeituraAsync(leitura, token);

        var response = new LeituraSensorResponse
        {
            Id = result.Id,
            Timestamp = result.Timestamp,
            Temperatura = result.Temperatura,
            Ph = result.Ph,
            Oxigenio = result.Oxigenio,
            NivelAgua = result.NivelAgua
        };

        return Results.Created($"/api/leituras/{result.Id}", response);
    }
}