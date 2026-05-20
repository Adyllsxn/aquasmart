namespace Aquasmart.Server.Source.Infrastructure.Data.Configurations;
public class AlertaEntityConfiguration : IEntityTypeConfiguration<AlertaEntity>
{
    public void Configure(EntityTypeBuilder<AlertaEntity> builder)
    {
        builder.ToTable("Alertas");
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Mensagem)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Tipo)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(x => x.Gravidade)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(x => x.Timestamp)
               .IsRequired();

        builder.Property(x => x.Lida)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(x => x.ValorRegistado)
               .IsRequired()
               .HasPrecision(5, 2);

        // Foreign Key
        builder.Property(x => x.LeituraSensorId)
               .IsRequired();

        // Relationship
        builder.HasOne(x => x.LeituraSensor)
               .WithMany(x => x.Alertas)
               .HasForeignKey(x => x.LeituraSensorId)
               .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.Timestamp)
               .HasDatabaseName("IX_Alertas_Timestamp");

        builder.HasIndex(x => new { x.Lida, x.Timestamp })
               .HasDatabaseName("IX_Alertas_Lida_Timestamp");
    }
}