namespace Aquasmart.Web.Core.DTOs;
public class ComandoDto
{
    public Guid Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Acao { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string ExecutadoPor { get; set; } = string.Empty;
    public Guid? LeituraSensorId { get; set; }
}