namespace Aquasmart.Server.Source.Presentation.Common.Configs;
public static class Modules
{
    public static IServiceCollection AddInfrastruture (this IServiceCollection services, IConfiguration configuration)
    {
        #region Services
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAlertaService, AlertaService>();
        services.AddScoped<IComandoService, ComandoService>();
        services.AddScoped<ILeituraSensorService, LeituraSensorService>();
        #endregion


        #region DbConnection
        var connectionDb = configuration.GetConnectionString(DatabaseConnections.Postgres)
            ?? throw new InvalidOperationException("Database connection not configured.");

        services.AddDbContext<AppDbContext>(opt =>opt.UseNpgsql(connectionDb, x => x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        #endregion

        return services;
    }
}
