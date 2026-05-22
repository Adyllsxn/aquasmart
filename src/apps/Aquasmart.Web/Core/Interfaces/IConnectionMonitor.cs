namespace Aquasmart.Web.Core.Interfaces;
public interface IConnectionMonitor
{
    bool IsConnected { get; }
    event Action ConnectionStatusChanged;
    Task CheckConnectionAsync();
}
