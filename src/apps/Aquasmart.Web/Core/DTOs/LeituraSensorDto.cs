namespace Aquasmart.Web.Core.DTOs;
public class LeituraSensorDto
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Temperatura { get; set; }
    public decimal Ph { get; set; }
    public decimal Oxigenio { get; set; }
    public decimal NivelAgua { get; set; }
}