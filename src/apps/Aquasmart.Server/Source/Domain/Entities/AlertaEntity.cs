namespace Aquasmart.Server.Source.Domain.Entities;
public sealed class AlertaEntity : EntityBase
{
    #region Properties
    public string Mensagem { get; private set; } = string.Empty;
    public TipoSensor Tipo { get; private set; }
    public Gravidade Gravidade { get; private set; }
    public DateTime Timestamp { get; private set; }
    public bool Lida { get; private set; }
    public decimal ValorRegistado { get; private set; }
    
    // Foreign Key
    public Guid LeituraSensorId { get; private set; }
    #endregion

    #region Navigation Properties
    public LeituraSensorEntity? LeituraSensor { get; private set; }
    #endregion

    #region Constructors
    private AlertaEntity() { }

    public AlertaEntity(TipoSensor tipo, string mensagem, Gravidade gravidade, decimal valorRegistado, Guid leituraSensorId)
    {
        Validate(mensagem);
        DomainValidator.When(leituraSensorId == Guid.Empty, "LeituraSensorId cannot be empty");
        
        Tipo = tipo;
        Mensagem = mensagem;
        Gravidade = gravidade;
        ValorRegistado = valorRegistado;
        LeituraSensorId = leituraSensorId;
        Timestamp = DateTime.UtcNow;
        Lida = false;
    }
    #endregion

    #region Domain Methods
    public void MarcarComoLida()
    {
        Lida = true;
    }

    public void AtualizarGravidade(Gravidade novaGravidade)
    {
        Gravidade = novaGravidade;
    }
    #endregion

    #region Private Methods
    private void Validate(string mensagem)
    {
        DomainValidator.NotEmpty(mensagem, "Message");
        DomainValidator.When(mensagem.Length > 200, "Message cannot exceed 200 characters");
    }
    #endregion
}