namespace Aquasmart.Server.Source.Presentation.Hubs;

public class LeituraHub : Hub
{
    public async Task EnviarLeitura(object leitura)
    {
        await Clients.All.SendAsync("NovaLeitura", leitura);
    }

    public async Task EnviarTodasLeituras(List<object> leituras)
    {
        await Clients.All.SendAsync("ListaLeituras", leituras);
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"📡 LeituraHub - Cliente conectado: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"📡 LeituraHub - Cliente desconectado: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}