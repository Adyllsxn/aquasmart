namespace Aquasmart.Web.Core.Services;

public class AlertasService
{
    private HubConnection? _hubConnection;
    private readonly string _hubUrl;
    private readonly HttpClient _httpClient;
    
    public event Action<AlertaDto>? OnNovoAlerta;
    public event Action<Guid>? OnAlertaLido;
    public event Action<AlertasStatsModels>? OnEstatisticasAtualizadas;
    public event Action? OnConnected;
    public event Action<string?>? OnDisconnected;
    
    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
    
    public AlertasService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var baseUrl = httpClient.BaseAddress?.ToString().TrimEnd('/');
        _hubUrl = $"{baseUrl}/alertasHub";
        Console.WriteLine($"📡 AlertasService - Hub URL: {_hubUrl}");
    }
    
    public async Task StartAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect(new AlertasRetryPolicy())
            .Build();
        
        _hubConnection.On<AlertaDto>("NovoAlerta", (alerta) =>
        {
            Console.WriteLine($"🔔 Novo alerta: {alerta.Mensagem}");
            OnNovoAlerta?.Invoke(alerta);
        });
        
        _hubConnection.On<Guid>("AlertaLido", (id) =>
        {
            Console.WriteLine($"✅ Alerta lido: {id}");
            OnAlertaLido?.Invoke(id);
        });
        
        _hubConnection.On<AlertasStatsModels>("EstatisticasAtualizadas", (stats) =>
        {
            Console.WriteLine($"📊 Estatísticas: Total={stats.Total}, NaoLidos={stats.NaoLidos}");
            OnEstatisticasAtualizadas?.Invoke(stats);
        });
        
        _hubConnection.On<string>("AlertasMarcadosLidos", (timestamp) =>
        {
            Console.WriteLine($"📋 Todos alertas marcados como lidos: {timestamp}");
        });
        
        _hubConnection.Closed += async (error) =>
        {
            Console.WriteLine($"🔴 AlertasHub Closed: {error?.Message}");
            OnDisconnected?.Invoke(error?.Message);
            await Task.CompletedTask;
        };
        
        _hubConnection.Reconnecting += async (error) =>
        {
            Console.WriteLine($"🟡 AlertasHub Reconnecting: {error?.Message}");
            await Task.CompletedTask;
        };
        
        _hubConnection.Reconnected += async (connectionId) =>
        {
            Console.WriteLine($"🟢 AlertasHub Reconnected: {connectionId}");
            OnConnected?.Invoke();
            await Task.CompletedTask;
        };
        
        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine($"✅ AlertasHub Connected! ConnectionId: {_hubConnection.ConnectionId}");
            OnConnected?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ AlertasHub Error: {ex.Message}");
            OnDisconnected?.Invoke(ex.Message);
        }
    }
    
    public async Task StopAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
    
    public HubConnectionState GetConnectionState()
    {
        return _hubConnection?.State ?? HubConnectionState.Disconnected;
    }
    
    // API Methods
    public async Task<List<AlertaDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AlertaDto>>("/api/alertas") ?? new();
    }
    
    public async Task<List<AlertaDto>> GetNaoLidosAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AlertaDto>>("/api/alertas/nao-lidos") ?? new();
    }
    
    public async Task MarcarComoLidaAsync(Guid id)
    {
        await _httpClient.PutAsync($"/api/alertas/{id}/marcar-lida", null);
    }
    
    public async Task MarcarTodosComoLidosAsync()
    {
        await _httpClient.PutAsync("/api/alertas/marcar-todos-lidos", null);
    }
}

public class AlertasRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        if (retryContext.PreviousRetryCount >= 10)
            return null;
        
        return TimeSpan.FromSeconds(Math.Min(32, Math.Pow(2, retryContext.PreviousRetryCount)));
    }
}

// DTOs
public class AlertaDto
{
    public Guid Id { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public int Tipo { get; set; }
    public int Gravidade { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Lida { get; set; }
    public decimal ValorRegistado { get; set; }
    public Guid LeituraSensorId { get; set; }
}