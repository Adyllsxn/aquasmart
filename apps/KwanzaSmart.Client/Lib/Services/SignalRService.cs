namespace KwanzaSmart.Client.Lib.Services;
public class SignalRService : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly ILogger<SignalRService> _logger;

    public SignalRService(ILogger<SignalRService> logger)
    {
        _logger = logger;
    }

    // Eventos para o frontend
    public event Action<LeituraSensorDto>? OnNovaLeitura;
    public event Action<AlertaDto>? OnNovoAlerta;
    public event Action<ComandoDto>? OnComandoExecutado;
    public event Action<bool>? OnConnectionStateChanged;

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public async Task StartAsync(string hubUrl)
    {
        if (_hubConnection != null)
        {
            await StopAsync();
        }

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        // Registrar handlers
        _hubConnection.On<LeituraSensorDto>("ReceiveNovaLeitura", leitura =>
        {
            _logger.LogInformation("📊 Nova leitura: {Temp}°C", leitura.Temperatura);
            OnNovaLeitura?.Invoke(leitura);
        });

        _hubConnection.On<AlertaDto>("ReceiveNovoAlerta", alerta =>
        {
            _logger.LogWarning("⚠️ Novo alerta: {Mensagem}", alerta.Mensagem);
            OnNovoAlerta?.Invoke(alerta);
        });

        _hubConnection.On<ComandoDto>("ReceiveComandoExecutado", comando =>
        {
            _logger.LogInformation("🔧 Comando executado: {Tipo}", comando.Tipo);
            OnComandoExecutado?.Invoke(comando);
        });

        // Estado da conexão
        _hubConnection.Reconnecting += (error) =>
        {
            _logger.LogWarning("Reconnecting...");
            OnConnectionStateChanged?.Invoke(false);
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += (connectionId) =>
        {
            _logger.LogInformation("Reconnected!");
            OnConnectionStateChanged?.Invoke(true);
            return Task.CompletedTask;
        };

        _hubConnection.Closed += (error) =>
        {
            _logger.LogWarning("Connection closed");
            OnConnectionStateChanged?.Invoke(false);
            return Task.CompletedTask;
        };

        try
        {
            await _hubConnection.StartAsync();
            _logger.LogInformation("✅ SignalR connected to {HubUrl}", hubUrl);
            OnConnectionStateChanged?.Invoke(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error connecting to SignalR");
            OnConnectionStateChanged?.Invoke(false);
        }
    }

    public async Task StopAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
    }
}