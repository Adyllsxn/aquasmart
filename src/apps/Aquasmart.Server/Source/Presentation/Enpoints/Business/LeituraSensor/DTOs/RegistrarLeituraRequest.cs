namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.LeituraSensor.DTOs;
public class RegistrarLeituraRequest
{
    public decimal Temperatura { get; set; }
    public decimal Ph { get; set; }
    public decimal Oxigenio { get; set; }
    public decimal NivelAgua { get; set; }
}