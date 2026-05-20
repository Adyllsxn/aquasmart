namespace Aquasmart.Server.Source.Infrastructure.Services;
public class AlertaService : IAlertaService
{
    #region Dependencies
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;

    public AlertaService(AppDbContext context, IUnitOfWork uow)
    {
        _context = context;
        _uow = uow;
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
        return alerta;
    }

    public async Task MarcarComoLidaAsync(Guid id, CancellationToken cancellationToken)
    {
        var alerta = await _context.Alertas.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (alerta != null)
        {
            alerta.MarcarComoLida();
            await _uow.SaveChangesAsync(cancellationToken);
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
    }

    public async Task AtualizarGravidadeAsync(Guid id, Gravidade novaGravidade, CancellationToken cancellationToken)
    {
        var alerta = await _context.Alertas.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (alerta != null)
        {
            alerta.AtualizarGravidade(novaGravidade);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IReadOnlyList<AlertaEntity>> ProcessarRegrasAsync(LeituraSensorEntity leitura, CancellationToken cancellationToken)
    {
        var alertas = new List<AlertaEntity>();
        
        // Regra: Temperatura > 28°C
        if (leitura.Temperatura > 28)
        {
            alertas.Add(new AlertaEntity(
                TipoSensor.Temperatura,
                $"Temperatura alta: {leitura.Temperatura}°C",
                Gravidade.Vermelho,
                leitura.Temperatura,
                leitura.Id));
        }
        
        // Regra: pH < 6.5
        if (leitura.Ph < 6.5m)
        {
            alertas.Add(new AlertaEntity(
                TipoSensor.Ph,
                $"pH baixo: {leitura.Ph}",
                Gravidade.Amarelo,
                leitura.Ph,
                leitura.Id));
        }
        
        // Regra: Oxigénio < 5 mg/L
        if (leitura.Oxigenio < 5)
        {
            alertas.Add(new AlertaEntity(
                TipoSensor.Oxigenio,
                $"Oxigénio baixo: {leitura.Oxigenio} mg/L",
                Gravidade.Vermelho,
                leitura.Oxigenio,
                leitura.Id));
        }
        
        // Regra: Nível < 30%
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
}