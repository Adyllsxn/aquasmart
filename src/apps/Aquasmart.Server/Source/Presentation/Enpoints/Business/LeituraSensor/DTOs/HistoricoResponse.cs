namespace Aquasmart.Server.Source.Presentation.Enpoints.Business.LeituraSensor.DTOs;
public class HistoricoResponse
{
    public int PeriodoMinutos { get; set; }
    public int TotalLeituras { get; set; }
    public List<LeituraSensorResponse> Leituras { get; set; } = new();
}