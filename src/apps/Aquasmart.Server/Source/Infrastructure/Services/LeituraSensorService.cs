namespace Aquasmart.Server.Source.Infrastructure.Services;
public class LeituraSensorService : ILeituraSensorService
{
    #region Dependencies
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;

    public LeituraSensorService(AppDbContext context, IUnitOfWork uow)
    {
        _context = context;
        _uow = uow;
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
        await _context.Leituras.AddAsync(leitura, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return leitura;
    }

    public async Task RegistrarLeiturasEmLoteAsync(IEnumerable<LeituraSensorEntity> leituras, CancellationToken cancellationToken)
    {
        await _context.Leituras.AddRangeAsync(leituras, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
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