namespace Aquasmart.Server.Source.Domain.Abstractions.Services;
public interface ILeituraSensorService : ILeituraSensorReadService, ILeituraSensorWriteService { }

public interface ILeituraSensorReadService
{
    Task<IReadOnlyList<LeituraSensorEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<LeituraSensorEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<LeituraSensorEntity>> GetHistoricoAsync(int ultimosMinutos, CancellationToken cancellationToken);
    Task<LeituraSensorEntity?> GetUltimaLeituraAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<LeituraSensorEntity>> GetPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken);
    Task<decimal> GetMediaTemperaturaAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken);
    Task<decimal> GetMediaPhAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken);
}

public interface ILeituraSensorWriteService
{
    Task<LeituraSensorEntity> RegistrarLeituraAsync(LeituraSensorEntity leitura, CancellationToken cancellationToken);
    Task RegistrarLeiturasEmLoteAsync(IEnumerable<LeituraSensorEntity> leituras, CancellationToken cancellationToken);
    Task<bool> DeleteLeituraAntigaAsync(DateTime cutoff, CancellationToken cancellationToken);
}