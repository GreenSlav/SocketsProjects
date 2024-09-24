using System.Net.WebSockets;
using System.Text;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<WebSocket> connectedClients = new List<WebSocket>();  // Список подключённых клиентов

app.UseWebSockets();

app.Map("/chat", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        Console.WriteLine("WebSocket соединение установлено");
        connectedClients.Add(webSocket);  // Добавляем нового клиента в список

        try
        {
            await HandleWebSocketCommunication(webSocket);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            connectedClients.Remove(webSocket);  // Удаляем клиента при закрытии соединения
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", CancellationToken.None);
        }
    }
    else
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("WebSocket connection expected.");
    }
});

async Task HandleWebSocketCommunication(WebSocket webSocket)
{
    var buffer = new byte[1024 * 4];
    WebSocketReceiveResult result;

    do
    {
        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        if (result.MessageType == WebSocketMessageType.Text)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"Сообщение: {message}");

            // Рассылка сообщения всем подключённым клиентам
            foreach (var client in connectedClients.ToList())  // Чтобы избежать изменения списка во время итерации
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

    } while (!result.CloseStatus.HasValue);

    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
}

app.Run();
