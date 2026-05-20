namespace Aquasmart.Server.Source.Infrastructure.Data.Context;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<AlertaEntity> Alertas { get; set; } = null!;
    public DbSet<ComandoEntity> Comandos { get; set; } = null!;
    public DbSet<LeituraSensorEntity> Leituras { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
}
