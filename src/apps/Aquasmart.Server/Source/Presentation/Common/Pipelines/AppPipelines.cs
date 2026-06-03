namespace Aquasmart.Server.Source.Presentation.Common.Pipelines;
public static class AppPipelines
{
    public static void UseAppPipelines(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>(); 
        app.UseHttpsRedirection(); 
        app.UseCorsExtensions(); 
        app.UseUiDocExtensions(); 
        app.MapEndpoints();
        app.UseSignalRExtensions();
        app.Run();
    }
}