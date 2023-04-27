using ChatPrototype.UsefulTools;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authentication;
using System;

namespace ChatPrototype.Controllers
{
    public class WebsocketController : Controller
    {

        static MessageTool messageTool = new();
        static List<WebSocket> userList = new();

        //receive websocket request for chating
        public async Task AddSocket()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                userList.Add(webSocket);
                AuthenticateResult authenticate = ControllerContext.HttpContext.AuthenticateAsync().Result;
                if (authenticate.Succeeded)
                {
                    await Chating(webSocket);
                }
                else
                {
                    await Listening(webSocket);
                }
            } else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }


        //The process of chating
        [NonAction]
        private static async Task Chating(WebSocket websocket)
        {
            byte[] buffer = new byte[1024];
            //Добавть отдельный метод для этого блока
            foreach (byte[] message in messageTool.GetBuffer())
            {
                await websocket.SendAsync(new ArraySegment<byte>(message),
                        WebSocketMessageType.Text,
                        WebSocketMessageFlags.EndOfMessage,
                        CancellationToken.None);
            }
            //---------------------------------------------
            WebSocketReceiveResult receiveResult = await websocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                byte[] sendMes = messageTool.AddByteArray(buffer);
                foreach (WebSocket user in userList)
                {
                    await websocket.SendAsync(
                        new ArraySegment<byte>(sendMes),
                        WebSocketMessageType.Text,
                        receiveResult.EndOfMessage,
                        CancellationToken.None);
                }

                Array.Clear(buffer);
  
                receiveResult = await websocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await websocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
            userList.Remove(websocket);
        }

        //This method is created for nonautherizied users
        [NonAction]
        private static async Task Listening(WebSocket websocket)
        {
            byte[] buffer = new byte[1024];
            foreach (byte[] message in messageTool.GetBuffer())
            {
                await websocket.SendAsync(new ArraySegment<byte>(message),
                        WebSocketMessageType.Text,
                        WebSocketMessageFlags.EndOfMessage,
                        CancellationToken.None);
            }
            WebSocketReceiveResult receiveResult = await websocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                receiveResult = await websocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await websocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
            userList.Remove(websocket);
        }
    }
}
