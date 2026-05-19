namespace Aquasmart.Server.Source.Domain.Entities;
public sealed class LeituraSensorEntity : EntityBase
{
    #region Properties
    public DateTime Timestamp { get; private set; }
    public decimal Temperatura { get; private set; }
    public decimal Ph { get; private set; }
    public decimal Oxigenio { get; private set; }
    public decimal NivelAgua { get; private set; }
    #endregion

    #region Navigation Properties
    private readonly List<AlertaEntity> _alertas = new();
    public IReadOnlyCollection<AlertaEntity> Alertas => _alertas.AsReadOnly();
    
    private readonly List<ComandoEntity> _comandos = new();
    public IReadOnlyCollection<ComandoEntity> Comandos => _comandos.AsReadOnly();
    #endregion

    #region Constructors
    private LeituraSensorEntity() { }

    public LeituraSensorEntity(decimal temperatura, decimal ph, decimal oxigenio, decimal nivelAgua)
    {
        Validate(temperatura, ph, oxigenio, nivelAgua);
        
        Timestamp = DateTime.UtcNow;
        Temperatura = temperatura;
        Ph = ph;
        Oxigenio = oxigenio;
        NivelAgua = nivelAgua;
    }
    #endregion

    #region Domain Methods
    public void AdicionarAlerta(AlertaEntity alerta)
    {
        _alertas.Add(alerta);
    }
    
    public void AdicionarComando(ComandoEntity comando)
    {
        _comandos.Add(comando);
    }
    #endregion

    #region Private Methods
    private void Validate(decimal temperatura, decimal ph, decimal oxigenio, decimal nivelAgua)
    {
        DomainValidator.When(temperatura < -10 || temperatura > 50, "Temperature must be between -10°C and 50°C");
        DomainValidator.When(ph < 0 || ph > 14, "pH must be between 0 and 14");
        DomainValidator.When(oxigenio < 0 || oxigenio > 20, "Oxygen must be between 0 and 20 mg/L");
        DomainValidator.When(nivelAgua < 0 || nivelAgua > 100, "Water level must be between 0% and 100%");
    }
    #endregion
}