namespace Aquasmart.Web.Core.Services;

public class LeituraService
{
    private HubConnection? _hubConnection;
    private readonly string _hubUrl;
    
    public event Action<Leitura>? OnNovaLeitura;
    public event Action<List<Leitura>>? OnListaLeituras;
    public event Action? OnConnected;
    public event Action<string?>? OnDisconnected;
    
    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
    
    public LeituraService(HttpClient httpClient)
    {
        var baseUrl = httpClient.BaseAddress?.ToString().TrimEnd('/');
        _hubUrl = $"{baseUrl}/leiturasHub";
        Console.WriteLine($"📡 LeituraService - Hub URL: {_hubUrl}");
    }
    
    public async Task StartAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect(new LeituraRetryPolicy())
            .Build();
        
        // Receber nova leitura individual
        _hubConnection.On<Leitura>("NovaLeitura", (leitura) =>
        {
            Console.WriteLine($"📊 Nova leitura: Temp={leitura.Temperatura}°C, pH={leitura.Ph}");
            OnNovaLeitura?.Invoke(leitura);
        });
        
        // Receber lista completa
        _hubConnection.On<List<Leitura>>("ListaLeituras", (leituras) =>
        {
            Console.WriteLine($"📊 Lista de leituras recebida: {leituras.Count} registros");
            OnListaLeituras?.Invoke(leituras);
        });
        
        // Eventos de conexão
        _hubConnection.Closed += async (error) =>
        {
            Console.WriteLine($"🔴 LeituraHub Closed: {error?.Message}");
            OnDisconnected?.Invoke(error?.Message);
            await Task.CompletedTask;
        };
        
        _hubConnection.Reconnecting += async (error) =>
        {
            Console.WriteLine($"🟡 LeituraHub Reconnecting: {error?.Message}");
            await Task.CompletedTask;
        };
        
        _hubConnection.Reconnected += async (connectionId) =>
        {
            Console.WriteLine($"🟢 LeituraHub Reconnected: {connectionId}");
            OnConnected?.Invoke();
            await Task.CompletedTask;
        };
        
        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine($"✅ LeituraHub Connected! ConnectionId: {_hubConnection.ConnectionId}");
            OnConnected?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ LeituraHub Error: {ex.Message}");
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
    
    public async Task EnviarLeitura(Leitura leitura)
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.InvokeAsync("EnviarLeitura", leitura);
        }
    }
}

public class LeituraRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        if (retryContext.PreviousRetryCount >= 10)
            return null;
        
        return TimeSpan.FromSeconds(Math.Min(32, Math.Pow(2, retryContext.PreviousRetryCount)));
    }
}