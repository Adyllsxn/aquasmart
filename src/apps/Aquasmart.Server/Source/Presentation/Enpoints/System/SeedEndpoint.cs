namespace Aquasmart.Server.Source.Presentation.Enpoints.System;
public class SeedEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/system/seed", HandleAsync)
           .WithSummary("Gerar dados de teste simulando o PIC")
           .WithTags("System");

    private static async Task<IResult> HandleAsync(
        [FromServices] ILeituraSensorService service,
        CancellationToken token)
    {
        var random = new Random();
        
        for (int i = 0; i < 50; i++)
        {
            var leitura = new LeituraSensorEntity(
                temperatura: 20 + random.Next(0, 150) / 10m,
                ph: 6 + random.Next(0, 20) / 10m,
                oxigenio: 4 + random.Next(0, 40) / 10m,
                nivelAgua: 30 + random.Next(0, 70)
            );
            await service.RegistrarLeituraAsync(leitura, token);
        }
        
        return Results.Ok(new { message = "50 leituras geradas com sucesso!" });
    }
}