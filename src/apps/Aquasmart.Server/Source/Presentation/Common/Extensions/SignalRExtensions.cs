namespace Aquasmart.Server.Source.Presentation.Common.Extensions;
public static class SignalRExtensions
{
    public static void AddSignalRExtensions(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
    }

    public static void UseSignalRExtensions(this WebApplication app)
    {
        app.MapHub<AquariumHub>("/aquarium-hub");
    }
}