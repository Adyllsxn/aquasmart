namespace Aquasmart.Server.Source.Infrastructure.Data.Configurations;
public class LeituraSensorEntityConfiguration : IEntityTypeConfiguration<LeituraSensorEntity>
{
    public void Configure(EntityTypeBuilder<LeituraSensorEntity> builder)
    {
        builder.ToTable("LeiturasSensor");
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Timestamp)
               .IsRequired();

        builder.Property(x => x.Temperatura)
               .IsRequired()
               .HasPrecision(5, 2);

        builder.Property(x => x.Ph)
               .IsRequired()
               .HasPrecision(3, 1);

        builder.Property(x => x.Oxigenio)
               .IsRequired()
               .HasPrecision(4, 2);

        builder.Property(x => x.NivelAgua)
               .IsRequired()
               .HasPrecision(5, 2);

        // Relationships
        builder.HasMany(x => x.Alertas)
               .WithOne(x => x.LeituraSensor)
               .HasForeignKey(x => x.LeituraSensorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Comandos)
               .WithOne(x => x.LeituraSensor)
               .HasForeignKey(x => x.LeituraSensorId)
               .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.Timestamp)
               .HasDatabaseName("IX_LeiturasSensor_Timestamp");
    }
}