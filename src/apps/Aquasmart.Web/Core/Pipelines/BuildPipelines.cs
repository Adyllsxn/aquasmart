namespace Aquasmart.Web.Core.Pipelines;
public static class BuildPipelines
{
    public static void AddBuildPipelines(this WebAssemblyHostBuilder builder)
    {
        var apiUrl = builder.HostEnvironment.BaseAddress.Contains("https") 
        ? "https://localhost:7202"
        : "http://localhost:5205";
    
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

        builder.Services.AddScoped<SignalRService>(); 
        builder.Services.AddScoped<LeituraService>();
        builder.Services.AddScoped<ConnectionService>();
        builder.Services.AddScoped<AlertasService>();
        builder.Services.AddScoped<HistoricoService>();
        

        builder.Services
            .AddBlazorise()
            .AddTailwindProviders()
            .AddFontAwesomeIcons();
    }
}