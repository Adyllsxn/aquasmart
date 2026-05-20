namespace Aquasmart.Server.Source.Presentation.Enpoints.System;
public class SystemEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/", () => "Aquasmart")
           .WithSummary("App Name")
           .WithTags("System");
}
