namespace KwanzaSmart.Client.Lib.DTOs;
public class AlertaDto
{
    public Guid Id { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Gravidade { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool Lida { get; set; }
    public decimal ValorRegistado { get; set; }
    public Guid LeituraSensorId { get; set; }
}