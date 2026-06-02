namespace Aquasmart.Web.Core.Services;
public class HistoricoService
{
    private readonly HttpClient _httpClient;
    
    public HistoricoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Leitura>> GetAllLeiturasAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Leitura>>("/api/leituras") ?? new();
    }
    
    public async Task<List<Leitura>> GetLeiturasPorPeriodo(DateTime inicio, DateTime fim)
    {
        return await _httpClient.GetFromJsonAsync<List<Leitura>>($"/api/leituras/periodo?inicio={inicio:O}&fim={fim:O}") ?? new();
    }
    
    public async Task<List<AlertaDto>> GetAlertasPorPeriodo(DateTime inicio, DateTime fim)
    {
        return await _httpClient.GetFromJsonAsync<List<AlertaDto>>($"/api/alertas/periodo?inicio={inicio:O}&fim={fim:O}") ?? new();
    }
}