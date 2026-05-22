namespace Aquasmart.Web.Core.Services;

public class SignalRService
{
    private HubConnection? _hubConnection;
    private readonly string _hubUrl;
    
    public event Action? OnConnected;
    public event Action<string?>? OnDisconnected;
    public event Action? OnReconnecting;
    public event Action<string?>? OnReconnected;
    
    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
    
    public SignalRService(HttpClient httpClient)
    {
        var baseUrl = httpClient.BaseAddress?.ToString().TrimEnd('/');
        _hubUrl = $"{baseUrl}/sensorhub";
        Console.WriteLine($"🔌 SignalR Hub URL: {_hubUrl}");
    }
    
    public async Task StartAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect(new SignalRRetryPolicy())
            .Build();
        
        _hubConnection.Closed += async (error) =>
        {
            Console.WriteLine($"🔴 SignalR Closed: {error?.Message}");
            OnDisconnected?.Invoke(error?.Message);
            await Task.CompletedTask;
        };
        
        _hubConnection.Reconnecting += async (error) =>
        {
            Console.WriteLine($"🟡 SignalR Reconnecting: {error?.Message}");
            OnReconnecting?.Invoke();
            await Task.CompletedTask;
        };
        
        _hubConnection.Reconnected += async (connectionId) =>
        {
            Console.WriteLine($"🟢 SignalR Reconnected: {connectionId}");
            OnReconnected?.Invoke(connectionId);
            await Task.CompletedTask;
        };
        
        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine($"✅ SignalR Connected! ConnectionId: {_hubConnection.ConnectionId}");
            OnConnected?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ SignalR Error: {ex.Message}");
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
}

// ✅ Classe RetryPolicy dentro do mesmo namespace
public class SignalRRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        // Máximo de 10 tentativas
        if (retryContext.PreviousRetryCount >= 10)
            return null;
        
        // Exponential backoff: 2s, 4s, 8s, 16s, 32s...
        var delay = TimeSpan.FromSeconds(Math.Min(32, Math.Pow(2, retryContext.PreviousRetryCount)));
        return delay;
    }
}