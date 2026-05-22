namespace Aquasmart.Web.Core.Models;
public class Leitura
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double Temperatura { get; set; }
    public double Ph { get; set; }
    public double Oxigenio { get; set; }
    public double NivelAgua { get; set; }
}