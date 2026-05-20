namespace Aquasmart.Server.Source.Presentation.Common.Extensions;
public static class UiDocExtensions
{
    public static void AddUiDocExtensions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
    }
    public static void UseUiDocExtensions(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference();
            app.MapOpenApi();
        }
    }
}