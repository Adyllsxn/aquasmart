namespace Aquasmart.Server.Source.Presentation.Common.Extensions;
public static class ModulesExtensions
{
    public static void AddModulesExtensions(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastruture(builder.Configuration);
    }
}