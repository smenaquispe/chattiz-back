using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;
namespace chattiz_back.Utils;

public class WebSocketHandler
{

    private readonly RequestDelegate _next;
    private readonly ConcurrentDictionary<string, WebSocket> _connectedUsers = new ConcurrentDictionary<string, WebSocket>();

    public WebSocketHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // En este caso no es una petición de WebSocket
        if(!context.WebSockets.IsWebSocketRequest)
        {
            await _next(context);
            return;
        }

        Console.WriteLine("WebSocket request");
        // aqui si es una petición de WebSocket
        var ct = context.RequestAborted;
        using var socket = await context.WebSockets.AcceptWebSocketAsync();
        Console.WriteLine("WebSocket accepted " + socket.ToString());

        var message = await RecieveStringsAsync(socket, ct);

        if(message == null)
        {
            return;
        }

        if(message.Contains(" --!-- "))
        {
            string[] parts = message.Split(" --!-- ");

            /* Aqui se debe de hacer la lógica de los mensajes */
            /* Enviar a la base de datos */
            /* Puede haber la opcion conectar a la app */
            /* Puede haber la opcion de enviar un mensaje */

            switch(parts[0])
            {
                case "connect":
                    Console.WriteLine("Connected: " + parts[1]);
                    if(_connectedUsers.ContainsKey(parts[1]))
                    {
                        Console.WriteLine("User already connected: " + parts[1]);   
                        await SendStringAsync(socket, "error" + " --!-- " + "User already connected", ct);
                        return;
                    }
                    _connectedUsers.TryAdd(parts[1], socket);
                    await SendStringAsync(socket, "connected Id: " + parts[1], ct);
                    break;
                case "disconnect":
                    Console.WriteLine("Disconnected: " + parts[1]);
                    _connectedUsers.TryRemove(parts[1], out _);
                    await SendStringAsync(socket, "disconnected Id: " + parts[1], ct);
                    break;
                case "message":
                    var chatId = parts[1];
                    var senderId = parts[2];
                    var content = parts[3];
                    var sentAt = DateTime.Now;
                    Console.WriteLine("Message: " + chatId + " --!-- " + senderId + " --!-- " + content + " --!-- " + sentAt);

                    List<string> users = new List<string>();
                    // desde las posicion 4 hasta el final son los ids de los usuarios
                    
                    string usersString = "";
                    for(int i = 4; i < parts.Length; i++)
                    {
                        if(parts[i] != senderId) {
                            users.Add(parts[i]);
                            usersString += parts[i] + " ";
                        }
                    }

                    Console.WriteLine("Users: " + usersString);

                    Console.WriteLine("Users: " + users.ToString());
                    if(users.Count() == 0)
                    {
                        await SendStringAsync(socket, "error" + " --!-- " + "No users to send the message", ct);
                        return;
                    }

                    foreach(var user in users)
                    {
                        // buscamos todos los usuarios conectados y enviamos el mensaje
                        if(_connectedUsers.TryGetValue(user, out var userSocket))
                        {
                            if(userSocket.State != WebSocketState.Open)
                            {
                                await SendStringAsync(socket, "error" + " --!-- " + "User is not connected", ct);
                                _connectedUsers.TryRemove(user, out _);
                            }
                            else
                            {
                                await SendStringAsync(userSocket, "message" + " --!-- " + chatId + " --!-- " + senderId + " --!-- " + content + " --!-- " + sentAt, ct);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }   
        }
    }


    private static async Task<string> RecieveStringsAsync(WebSocket socket, CancellationToken ct = default)
    {
        var buffer = new ArraySegment<byte>(new byte[8192]);
        using(var ms = new MemoryStream())
        {
            WebSocketReceiveResult result;
            do {
                ct.ThrowIfCancellationRequested();

                result = await socket.ReceiveAsync(buffer, ct);
                ms.Write(buffer.Array!, buffer.Offset, result.Count);
            }
            while(!result.EndOfMessage);

            ms.Seek(0, SeekOrigin.Begin);
            if(result.MessageType != WebSocketMessageType.Text)
            {
                throw new Exception("Unexpected message type: " + result.MessageType);
            }

            using(var reader = new StreamReader(ms, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }

    private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default)
    {
        var buffer = Encoding.UTF8.GetBytes(data);
        var segment = new ArraySegment<byte>(buffer);
        return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
    }

}

public static class WebSocketExtensions
{
    public static IApplicationBuilder UseWebSocketHandler(this WebApplication app)
    {
        return app.UseMiddleware<WebSocketHandler>();
    }
}
