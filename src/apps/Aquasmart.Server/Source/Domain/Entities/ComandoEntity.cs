namespace Aquasmart.Server.Source.Domain.Entities;
public sealed class ComandoEntity : EntityBase
{
    #region Properties
    public TipoComando Tipo { get; private set; }
    public AcaoComando Acao { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string ExecutadoPor { get; private set; } = string.Empty;
    
    // Foreign Key (nullable porque pode ser manual sem leitura associada)
    public Guid? LeituraSensorId { get; private set; }
    #endregion

    #region Navigation Properties
    public LeituraSensorEntity? LeituraSensor { get; private set; }
    #endregion

    #region Constructors
    private ComandoEntity() { }

    // Construtor para comando manual (sem leitura associada)
    public ComandoEntity(TipoComando tipo, AcaoComando acao, string executadoPor = "Manual")
    {
        Validate(executadoPor);
        
        Tipo = tipo;
        Acao = acao;
        ExecutadoPor = executadoPor;
        LeituraSensorId = null;
        Timestamp = DateTime.UtcNow;
    }
    
    // Construtor para comando automático (associado a uma leitura)
    public ComandoEntity(TipoComando tipo, AcaoComando acao, Guid leituraSensorId, string executadoPor = "Automático")
    {
        Validate(executadoPor);
        DomainValidator.When(leituraSensorId == Guid.Empty, "LeituraSensorId cannot be empty");
        
        Tipo = tipo;
        Acao = acao;
        ExecutadoPor = executadoPor;
        LeituraSensorId = leituraSensorId;
        Timestamp = DateTime.UtcNow;
    }
    #endregion

    #region Domain Methods
    public void AssociarLeitura(Guid leituraSensorId)
    {
        DomainValidator.When(leituraSensorId == Guid.Empty, "LeituraSensorId cannot be empty");
        LeituraSensorId = leituraSensorId;
    }
    #endregion

    #region Private Methods
    private void Validate(string executadoPor)
    {
        DomainValidator.NotEmpty(executadoPor, "Executed by");
        DomainValidator.When(executadoPor.Length > 50, "Executed by cannot exceed 50 characters");
    }
    #endregion
}