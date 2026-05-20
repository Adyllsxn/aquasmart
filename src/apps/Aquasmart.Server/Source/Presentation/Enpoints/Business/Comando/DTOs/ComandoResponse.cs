namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.Comando.DTOs;
public class ComandoResponse
{
    public Guid Id { get; set; }
    public TipoComando Tipo { get; set; }
    public AcaoComando Acao { get; set; }
    public DateTime Timestamp { get; set; }
    public string ExecutadoPor { get; set; } = string.Empty;
    public Guid? LeituraSensorId { get; set; }
}