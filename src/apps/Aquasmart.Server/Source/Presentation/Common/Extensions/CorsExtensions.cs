namespace Aquasmart.Server.Source.Presentation.Common.Extensions;
public static class CorsExtensions
{
    public static void AddCorsExtensions(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(x => x.AddPolicy(
            UrlCors.CorsPolicyNames,
            policy => policy
                .WithOrigins(UrlCors.Frontend)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
        ));
    }

    public static void UseCorsExtensions(this WebApplication app)
    {
        app.UseCors(UrlCors.CorsPolicyNames);
    }
}