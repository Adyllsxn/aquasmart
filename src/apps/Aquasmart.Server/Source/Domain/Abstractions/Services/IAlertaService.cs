namespace Aquasmart.Server.Source.Domain.Abstractions.Services;
public interface IAlertaService : IAlertaReadService, IAlertaWriteService { }

public interface IAlertaReadService
{
    Task<IReadOnlyList<AlertaEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<AlertaEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<AlertaEntity>> GetNaoLidosAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<AlertaEntity>> GetPorGravidadeAsync(Gravidade gravidade, CancellationToken cancellationToken);
    Task<IReadOnlyList<AlertaEntity>> GetPorTipoAsync(TipoSensor tipo, CancellationToken cancellationToken);
    Task<IReadOnlyList<AlertaEntity>> GetAlertasRecentesAsync(int ultimosMinutos, CancellationToken cancellationToken);
    Task<bool> ExisteAlertaNaoLidoPorTipoAsync(TipoSensor tipo, CancellationToken cancellationToken);
}

public interface IAlertaWriteService
{
    Task<AlertaEntity> CriarAlertaAsync(AlertaEntity alerta, CancellationToken cancellationToken);
    Task MarcarComoLidaAsync(Guid id, CancellationToken cancellationToken);
    Task MarcarTodosComoLidosAsync(CancellationToken cancellationToken);
    Task AtualizarGravidadeAsync(Guid id, Gravidade novaGravidade, CancellationToken cancellationToken);
    Task<IReadOnlyList<AlertaEntity>> ProcessarRegrasAsync(LeituraSensorEntity leitura, CancellationToken cancellationToken);
}