var builder = WebApplication.CreateBuilder(args);
    builder.AddBuildPipelines();

var app = builder.Build();
    app.UseAppPipelines();
    app.Run();