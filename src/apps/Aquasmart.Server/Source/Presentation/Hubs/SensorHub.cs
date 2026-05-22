namespace Aquasmart.Server.Source.Presentation.Hubs;
public class SensorHub : Hub
{
    public async Task SendSensorData(string data)
    {
        await Clients.All.SendAsync("ReceiveSensorData", data);
    }
    
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"✅ Cliente conectado: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"❌ Cliente desconectado: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}