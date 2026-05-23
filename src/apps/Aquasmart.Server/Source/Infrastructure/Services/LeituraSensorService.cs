namespace Aquasmart.Server.Source.Infrastructure.Services;
public class LeituraSensorService : ILeituraSensorService
{
    #region Dependencies
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;
    private readonly IAlertaService _alertaService;

    public LeituraSensorService(AppDbContext context, IUnitOfWork uow, IAlertaService alertaService)
    {
        _context = context;
        _uow = uow;
        _alertaService = alertaService;
    }
    #endregion

    #region Read Methods
    public async Task<IReadOnlyList<LeituraSensorEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Leituras
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<LeituraSensorEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Leituras
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<LeituraSensorEntity>> GetHistoricoAsync(int ultimosMinutos, CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-ultimosMinutos);
        return await _context.Leituras
            .Where(x => x.Timestamp >= cutoff)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<LeituraSensorEntity?> GetUltimaLeituraAsync(CancellationToken cancellationToken)
    {
        return await _context.Leituras
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<LeituraSensorEntity>> GetPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken)
    {
        return await _context.Leituras
            .Where(x => x.Timestamp >= inicio && x.Timestamp <= fim)
            .OrderBy(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetMediaTemperaturaAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken)
    {
        return await _context.Leituras
            .Where(x => x.Timestamp >= inicio && x.Timestamp <= fim)
            .AverageAsync(x => x.Temperatura, cancellationToken);
    }

    public async Task<decimal> GetMediaPhAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken)
    {
        return await _context.Leituras
            .Where(x => x.Timestamp >= inicio && x.Timestamp <= fim)
            .AverageAsync(x => x.Ph, cancellationToken);
    }
    #endregion

    #region Write Methods
    public async Task<LeituraSensorEntity> RegistrarLeituraAsync(LeituraSensorEntity leitura, CancellationToken cancellationToken)
    {
        // 1. Salvar a leitura
        await _context.Leituras.AddAsync(leitura, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        
        // 2. Processar regras e gerar alertas automaticamente
        var alertas = await _alertaService.ProcessarRegrasAsync(leitura, cancellationToken);
        
        // 3. Salvar os alertas gerados
        foreach (var alerta in alertas)
        {
            await _alertaService.CriarAlertaAsync(alerta, cancellationToken);
        }
        
        return leitura;
    }

    public async Task RegistrarLeiturasEmLoteAsync(IEnumerable<LeituraSensorEntity> leituras, CancellationToken cancellationToken)
    {
        await _context.Leituras.AddRangeAsync(leituras, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        
        // Processar alertas para cada leitura em lote
        foreach (var leitura in leituras)
        {
            var alertas = await _alertaService.ProcessarRegrasAsync(leitura, cancellationToken);
            foreach (var alerta in alertas)
            {
                await _alertaService.CriarAlertaAsync(alerta, cancellationToken);
            }
        }
    }

    public async Task<bool> DeleteLeituraAntigaAsync(DateTime cutoff, CancellationToken cancellationToken)
    {
        var oldReadings = _context.Leituras.Where(x => x.Timestamp < cutoff);
        _context.Leituras.RemoveRange(oldReadings);
        var deleted = await _uow.SaveChangesAsync(cancellationToken);
        return deleted > 0;
    }
    #endregion
}