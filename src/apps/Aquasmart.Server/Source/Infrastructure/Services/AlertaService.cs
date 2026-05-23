namespace Aquasmart.Server.Source.Infrastructure.Services;
public class AlertaService : IAlertaService
{
    #region Dependencies
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;
    private readonly IHubContext<AlertasHub> _hubContext;

    public AlertaService(AppDbContext context, IUnitOfWork uow, IHubContext<AlertasHub> hubContext)
    {
        _context = context;
        _uow = uow;
        _hubContext = hubContext;
    }
    #endregion

    #region Read Methods
    public async Task<IReadOnlyList<AlertaEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Alertas
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<AlertaEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Alertas
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<AlertaEntity>> GetNaoLidosAsync(CancellationToken cancellationToken)
    {
        return await _context.Alertas
            .Where(x => !x.Lida)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AlertaEntity>> GetPorGravidadeAsync(Gravidade gravidade, CancellationToken cancellationToken)
    {
        return await _context.Alertas
            .Where(x => x.Gravidade == gravidade)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AlertaEntity>> GetPorTipoAsync(TipoSensor tipo, CancellationToken cancellationToken)
    {
        return await _context.Alertas
            .Where(x => x.Tipo == tipo)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AlertaEntity>> GetAlertasRecentesAsync(int ultimosMinutos, CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-ultimosMinutos);
        return await _context.Alertas
            .Where(x => x.Timestamp >= cutoff)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExisteAlertaNaoLidoPorTipoAsync(TipoSensor tipo, CancellationToken cancellationToken)
    {
        return await _context.Alertas
            .AnyAsync(x => x.Tipo == tipo && !x.Lida, cancellationToken);
    }
    #endregion

    #region Write Methods
    public async Task<AlertaEntity> CriarAlertaAsync(AlertaEntity alerta, CancellationToken cancellationToken)
    {
        await _context.Alertas.AddAsync(alerta, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        
        await NotificarNovoAlerta(alerta);
        
        return alerta;
    }

    public async Task MarcarComoLidaAsync(Guid id, CancellationToken cancellationToken)
    {
        var alerta = await _context.Alertas.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (alerta != null)
        {
            alerta.MarcarComoLida();
            await _uow.SaveChangesAsync(cancellationToken);
            
            await NotificarAlertaLido(alerta);
        }
    }

    public async Task MarcarTodosComoLidosAsync(CancellationToken cancellationToken)
    {
        var alertasNaoLidas = await _context.Alertas.Where(x => !x.Lida).ToListAsync(cancellationToken);
        foreach (var alerta in alertasNaoLidas)
        {
            alerta.MarcarComoLida();
        }
        await _uow.SaveChangesAsync(cancellationToken);
        
        await _hubContext.Clients.All.SendAsync("AlertasMarcadosLidos", DateTime.UtcNow);
        await NotificarEstatisticas();
    }

    public async Task AtualizarGravidadeAsync(Guid id, Gravidade novaGravidade, CancellationToken cancellationToken)
    {
        var alerta = await _context.Alertas.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (alerta != null)
        {
            alerta.AtualizarGravidade(novaGravidade);
            await _uow.SaveChangesAsync(cancellationToken);
            await NotificarEstatisticas();
        }
    }

    public async Task<IReadOnlyList<AlertaEntity>> ProcessarRegrasAsync(LeituraSensorEntity leitura, CancellationToken cancellationToken)
    {
        var alertas = new List<AlertaEntity>();
        
        if (leitura.Temperatura > 28)
        {
            alertas.Add(new AlertaEntity(
                TipoSensor.Temperatura,
                $"Temperatura alta: {leitura.Temperatura}°C",
                Gravidade.Vermelho,
                leitura.Temperatura,
                leitura.Id));
        }
        
        if (leitura.Ph < 6.5m)
        {
            alertas.Add(new AlertaEntity(
                TipoSensor.Ph,
                $"pH baixo: {leitura.Ph}",
                Gravidade.Amarelo,
                leitura.Ph,
                leitura.Id));
        }
        
        if (leitura.Oxigenio < 5)
        {
            alertas.Add(new AlertaEntity(
                TipoSensor.Oxigenio,
                $"Oxigénio baixo: {leitura.Oxigenio} mg/L",
                Gravidade.Vermelho,
                leitura.Oxigenio,
                leitura.Id));
        }
        
        if (leitura.NivelAgua < 30)
        {
            alertas.Add(new AlertaEntity(
                TipoSensor.Nivel,
                $"Nível da água baixo: {leitura.NivelAgua}%",
                Gravidade.Vermelho,
                leitura.NivelAgua,
                leitura.Id));
        }
        
        return alertas;
    }
    #endregion

    #region Private SignalR Methods
    private async Task NotificarNovoAlerta(AlertaEntity alerta)
    {
        var alertaDto = new
        {
            alerta.Id,
            alerta.Mensagem,
            Tipo = (int)alerta.Tipo,
            Gravidade = (int)alerta.Gravidade,
            alerta.Timestamp,
            alerta.Lida,
            alerta.ValorRegistado,
            alerta.LeituraSensorId
        };
        
        await _hubContext.Clients.All.SendAsync("NovoAlerta", alertaDto);
        await NotificarEstatisticas();
    }
    
    private async Task NotificarAlertaLido(AlertaEntity alerta)
    {
        await _hubContext.Clients.All.SendAsync("AlertaLido", alerta.Id);
        await NotificarEstatisticas();
    }
    
    private async Task NotificarEstatisticas()
    {
        var total = await _context.Alertas.CountAsync();
        var naoLidos = await _context.Alertas.CountAsync(x => !x.Lida);
        var criticosNaoLidos = await _context.Alertas.CountAsync(x => !x.Lida && x.Gravidade == Gravidade.Vermelho);
        var amarelos = await _context.Alertas.CountAsync(x => x.Gravidade == Gravidade.Amarelo && !x.Lida);
        var lidos = total - naoLidos;
        
        var stats = new
        {
            Total = total,
            NaoLidos = naoLidos,
            Lidos = lidos,
            CriticosNaoLidos = criticosNaoLidos,
            Amarelos = amarelos
        };
        
        await _hubContext.Clients.All.SendAsync("EstatisticasAtualizadas", stats);
    }
    #endregion
}