namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.Alerta.DTOs;
public class AlertaResponse
{
    public Guid Id { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public TipoSensor Tipo { get; set; }
    public Gravidade Gravidade { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Lida { get; set; }
    public decimal ValorRegistado { get; set; }
    public Guid LeituraSensorId { get; set; }
}