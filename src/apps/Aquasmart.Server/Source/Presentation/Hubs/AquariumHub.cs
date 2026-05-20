namespace Aquasmart.Server.Source.Presentation.Hubs;
public class AquariumHub : Hub
{
    #region Dependencies
    private readonly ILeituraSensorService _leituraService;
    private readonly IAlertaService _alertaService;
    private readonly IComandoService _comandoService;
    private readonly ILogger<AquariumHub> _logger;

    public AquariumHub(
        ILeituraSensorService leituraService,
        IAlertaService alertaService,
        IComandoService comandoService,
        ILogger<AquariumHub> logger)
    {
        _leituraService = leituraService;
        _alertaService = alertaService;
        _comandoService = comandoService;
        _logger = logger;
    }
    #endregion

    #region Connection Management
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Cliente conectado: {ConnectionId}", Context.ConnectionId);
        
        // Envia a última leitura imediatamente para o novo cliente
        var ultimaLeitura = await _leituraService.GetUltimaLeituraAsync(CancellationToken.None);
        if (ultimaLeitura != null)
        {
            await Clients.Caller.SendAsync("ReceiveUltimaLeitura", ultimaLeitura);
        }
        
        // Envia alertas não lidos
        var alertasNaoLidos = await _alertaService.GetNaoLidosAsync(CancellationToken.None);
        if (alertasNaoLidos.Any())
        {
            await Clients.Caller.SendAsync("ReceiveAlertas", alertasNaoLidos);
        }
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Cliente desconectado: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
    #endregion

    #region Client Methods (Chamadas pelo Client)
    
    /// <summary>
    /// Cliente entra no grupo para receber atualizações em tempo real
    /// </summary>
    public async Task JoinDashboardGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Dashboard");
        _logger.LogInformation("Cliente {ConnectionId} entrou no grupo Dashboard", Context.ConnectionId);
    }
    
    public async Task LeaveDashboardGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Dashboard");
        _logger.LogInformation("Cliente {ConnectionId} saiu do grupo Dashboard", Context.ConnectionId);
    }
    
    /// <summary>
    /// Solicita histórico de leituras
    /// </summary>
    public async Task RequestHistorico(int minutos = 60)
    {
        var historico = await _leituraService.GetHistoricoAsync(minutos, CancellationToken.None);
        await Clients.Caller.SendAsync("ReceiveHistorico", historico);
    }
    
    /// <summary>
    /// Solicita comandos recentes
    /// </summary>
    public async Task RequestComandosRecentes(int minutos = 60)
    {
        var comandos = await _comandoService.GetComandosRecentesAsync(minutos, CancellationToken.None);
        await Clients.Caller.SendAsync("ReceiveComandosRecentes", comandos);
    }
    
    #endregion

    #region Server Methods (Chamadas pelo Backend)
    
    /// <summary>
    /// Envia nova leitura para todos os clientes conectados
    /// </summary>
    public async Task BroadcastNovaLeitura(LeituraSensorEntity leitura)
    {
        await Clients.Group("Dashboard").SendAsync("ReceiveNovaLeitura", leitura);
        _logger.LogInformation("Leitura broadcasted: Temp={Temp}°C, pH={Ph}, Oxigênio={Oxig} mg/L", 
            leitura.Temperatura, leitura.Ph, leitura.Oxigenio);
    }
    
    /// <summary>
    /// Envia novo alerta para todos os clientes conectados
    /// </summary>
    public async Task BroadcastNovoAlerta(AlertaEntity alerta)
    {
        await Clients.Group("Dashboard").SendAsync("ReceiveNovoAlerta", alerta);
        _logger.LogWarning("Alerta broadcasted: {Mensagem} (Gravidade: {Gravidade})", 
            alerta.Mensagem, alerta.Gravidade);
    }
    
    /// <summary>
    /// Envia comando executado para todos os clientes
    /// </summary>
    public async Task BroadcastComandoExecutado(ComandoEntity comando)
    {
        await Clients.Group("Dashboard").SendAsync("ReceiveComandoExecutado", comando);
        _logger.LogInformation("Comando broadcasted: {Tipo} - {Acao} ({ExecutadoPor})", 
            comando.Tipo, comando.Acao, comando.ExecutadoPor);
    }
    
    /// <summary>
    /// Alerta sobre valores críticos
    /// </summary>
    public async Task BroadcastAlertaCritico(string mensagem, decimal valor, string tipo)
    {
        await Clients.All.SendAsync("ReceiveAlertaCritico", new { mensagem, valor, tipo, timestamp = DateTime.UtcNow });
    }
    
    #endregion
}