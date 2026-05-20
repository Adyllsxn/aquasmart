namespace Aquasmart.Server.Source.Presentation.Common.Pipelines;
public static class BuildPipelines
{
    public static void AddBuildPipelines(this WebApplicationBuilder builder)
    {
        // 1. Módulos das camada 
        builder.AddModulesExtensions();

        // 2. Documentação (Scalar/OpenAPI)
        builder.AddUiDocExtensions();

        // 3. Segurança (CORS)
        builder.AddCorsExtensions();     

        // 4. Helpers e utilitários
        builder.AddSignalRExtensions();
    }
}