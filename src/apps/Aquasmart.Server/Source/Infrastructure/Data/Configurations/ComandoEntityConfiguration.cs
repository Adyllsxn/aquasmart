namespace Aquasmart.Server.Source.Infrastructure.Data.Configurations;
public class ComandoEntityConfiguration : IEntityTypeConfiguration<ComandoEntity>
{
    public void Configure(EntityTypeBuilder<ComandoEntity> builder)
    {
        builder.ToTable("Comandos");
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Tipo)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(x => x.Acao)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(x => x.Timestamp)
               .IsRequired();

        builder.Property(x => x.ExecutadoPor)
               .IsRequired()
               .HasMaxLength(50)
               .HasDefaultValue("Manual");

        // Foreign Key (nullable)
        builder.Property(x => x.LeituraSensorId)
               .IsRequired(false);

        // Relationship
        builder.HasOne(x => x.LeituraSensor)
               .WithMany(x => x.Comandos)
               .HasForeignKey(x => x.LeituraSensorId)
               .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.Timestamp)
               .HasDatabaseName("IX_Comandos_Timestamp");

        builder.HasIndex(x => new { x.Tipo, x.Timestamp })
               .HasDatabaseName("IX_Comandos_Tipo_Timestamp");
    }
}