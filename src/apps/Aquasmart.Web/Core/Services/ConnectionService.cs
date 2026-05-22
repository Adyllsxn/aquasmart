namespace Aquasmart.Web.Core.Services;
public class ConnectionService
{
    private HubConnection? _hubConnection;
    private bool _isConnected = false;
    
    public event Action? OnConnectionChanged;
    
    public bool IsConnected 
    { 
        get => _isConnected;
        private set
        {
            if (_isConnected != value)
            {
                _isConnected = value;
                OnConnectionChanged?.Invoke();
            }
        }
    }
    
    public async Task InitializeAsync(string hubUrl)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect(new ConnectionRetryPolicy())
            .Build();
        
        _hubConnection.On<string>("ReceiveMessage", (message) =>
        {
            // Processar mensagens se necessário
            Console.WriteLine($"📨 Message received: {message}");
        });
        
        _hubConnection.Reconnected += async (connectionId) =>
        {
            IsConnected = true;
            Console.WriteLine($"🟢 Reconnected: {connectionId}");
        };
        
        _hubConnection.Reconnecting += async (error) =>
        {
            IsConnected = false;
            Console.WriteLine($"🟡 Reconnecting: {error?.Message}");
        };
        
        _hubConnection.Closed += async (error) =>
        {
            IsConnected = false;
            Console.WriteLine($"🔴 Closed: {error?.Message}");
            await Task.Delay(5000);
            await StartAsync();
        };
        
        await StartAsync();
    }
    
    private async Task StartAsync()
    {
        try
        {
            if (_hubConnection?.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();
                IsConnected = true;
            }
            else if (_hubConnection?.State == HubConnectionState.Connected)
            {
                IsConnected = true;
            }
        }
        catch (Exception ex)
        {
            IsConnected = false;
            Console.WriteLine($"❌ Error starting connection: {ex.Message}");
            await Task.Delay(5000);
            await StartAsync();
        }
    }
    
    public async Task StopAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
            IsConnected = false;
        }
    }
    
    public HubConnectionState GetState()
    {
        return _hubConnection?.State ?? HubConnectionState.Disconnected;
    }
}

// ✅ Classe RetryPolicy para ConnectionService
public class ConnectionRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        // Exponential backoff: 2s, 4s, 8s, 16s, 32s
        var delay = TimeSpan.FromSeconds(Math.Min(32, Math.Pow(2, retryContext.PreviousRetryCount)));
        return delay;
    }
}