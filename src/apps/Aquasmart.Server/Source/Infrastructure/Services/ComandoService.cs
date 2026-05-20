namespace Aquasmart.Server.Source.Infrastructure.Services;
public class ComandoService : IComandoService
{
    #region Dependencies
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;

    public ComandoService(AppDbContext context, IUnitOfWork uow)
    {
        _context = context;
        _uow = uow;
    }
    #endregion

    #region Read Methods
    public async Task<IReadOnlyList<ComandoEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Comandos
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<ComandoEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Comandos
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ComandoEntity>> GetPorTipoAsync(TipoComando tipo, CancellationToken cancellationToken)
    {
        return await _context.Comandos
            .Where(x => x.Tipo == tipo)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ComandoEntity>> GetPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken)
    {
        return await _context.Comandos
            .Where(x => x.Timestamp >= inicio && x.Timestamp <= fim)
            .OrderBy(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<ComandoEntity?> GetUltimoComandoPorTipoAsync(TipoComando tipo, CancellationToken cancellationToken)
    {
        return await _context.Comandos
            .Where(x => x.Tipo == tipo)
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ComandoEntity>> GetComandosRecentesAsync(int ultimosMinutos, CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-ultimosMinutos);
        return await _context.Comandos
            .Where(x => x.Timestamp >= cutoff)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }
    #endregion

    #region Write Methods
    public async Task<ComandoEntity> ExecutarComandoAsync(ComandoEntity comando, CancellationToken cancellationToken)
    {
        await _context.Comandos.AddAsync(comando, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return comando;
    }

    public async Task<ComandoEntity> ExecutarComandoManualAsync(TipoComando tipo, AcaoComando acao, CancellationToken cancellationToken)
    {
        var comando = new ComandoEntity(tipo, acao, "Manual");
        await _context.Comandos.AddAsync(comando, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return comando;
    }

    public async Task<ComandoEntity> ExecutarComandoAutomaticoAsync(TipoComando tipo, AcaoComando acao, CancellationToken cancellationToken)
    {
        var comando = new ComandoEntity(tipo, acao, "Automático");
        await _context.Comandos.AddAsync(comando, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return comando;
    }
    #endregion
}