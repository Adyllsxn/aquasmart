namespace Aquasmart.Server.Source.Presentation.Common.Extensions;
public static class SignalRExtensions
{
    public static void AddSignalRExtensions(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
    }

    public static void UseSignalRExtensions(this WebApplication app)
    {
        app.MapHub<SensorHub>("/sensorhub");
        app.MapHub<LeituraHub>("/leiturasHub");
        Console.WriteLine("✅ SignalR Hubs mapeados: /sensorhub, /leiturasHub");
    }
}