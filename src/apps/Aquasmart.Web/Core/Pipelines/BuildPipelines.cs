namespace Aquasmart.Web.Core.Pipelines;
public static class BuildPipelines
{
    public static void AddBuildPipelines(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped(sp => new HttpClient 
        { 
            BaseAddress = new Uri("http://localhost:5205/")
        });

        builder.Services.AddScoped<SignalRService>(); 
        builder.Services.AddScoped<LeituraService>();
        builder.Services.AddScoped<ConnectionService>();

        builder.Services
            .AddBlazorise()
            .AddTailwindProviders()
            .AddFontAwesomeIcons();
    }
}