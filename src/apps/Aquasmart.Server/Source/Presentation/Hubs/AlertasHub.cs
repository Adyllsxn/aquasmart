namespace Aquasmart.Server.Source.Presentation.Hubs;

public class AlertasHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"🟢 Cliente conectado ao AlertasHub: {Context.ConnectionId}");
        await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"🔴 Cliente desconectado do AlertasHub: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}