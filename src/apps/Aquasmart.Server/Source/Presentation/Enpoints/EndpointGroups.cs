namespace Aquasmart.Server.Source.Presentation.Enpoints;
public static class EndpointGroups
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoint = app.MapGroup("");

        #region System
        endpoint.MapGroup("/") 
                .WithTags("Health Check")
                .MapEndpoint<SeedEndpoint>()
                .MapEndpoint<SystemEndpoint>();
        #endregion
        
        #region LeituraSensor
        app.MapGroup("/api/leituras")
           .WithTags("Leituras Sensor")
           .MapEndpoint<RegistrarLeituraEndpoint>()
           .MapEndpoint<GetAllLeiturasEndpoint>()
           .MapEndpoint<GetHistoricoEndpoint>()
           .MapEndpoint<GetUltimaLeituraEndpoint>();
        #endregion
        
        #region Alerta
        app.MapGroup("/api/alertas")
           .WithTags("Alertas")
           .MapEndpoint<GetAllAlertasEndpoint>()
           .MapEndpoint<GetNaoLidosEndpoint>()
           .MapEndpoint<MarcarComoLidaEndpoint>();
        #endregion

        #region Comando
        app.MapGroup("/api/comandos")
           .WithTags("Comandos")
           .MapEndpoint<ExecutarComandoManualEndpoint>()
           .MapEndpoint<GetComandosRecentesEndpoint>();
        #endregion
    }

    private static  IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint: IEndpoint
    {
        TEndpoint.MapEndpoint(app);
        return app;
    }
}
