namespace Aquasmart.Server.Source.Domain.Abstractions.Services;
public interface IComandoService : IComandoReadService, IComandoWriteService { }

public interface IComandoReadService
{
    Task<IReadOnlyList<ComandoEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<ComandoEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ComandoEntity>> GetPorTipoAsync(TipoComando tipo, CancellationToken cancellationToken);
    Task<IReadOnlyList<ComandoEntity>> GetPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken);
    Task<ComandoEntity?> GetUltimoComandoPorTipoAsync(TipoComando tipo, CancellationToken cancellationToken);
    Task<IReadOnlyList<ComandoEntity>> GetComandosRecentesAsync(int ultimosMinutos, CancellationToken cancellationToken);
}

public interface IComandoWriteService
{
    Task<ComandoEntity> ExecutarComandoAsync(ComandoEntity comando, CancellationToken cancellationToken);
    Task<ComandoEntity> ExecutarComandoManualAsync(TipoComando tipo, AcaoComando acao, CancellationToken cancellationToken);
    Task<ComandoEntity> ExecutarComandoAutomaticoAsync(TipoComando tipo, AcaoComando acao, CancellationToken cancellationToken);
}