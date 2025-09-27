using GameStreamEngine.WebSocket;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketDemo.WebSocket
{
    // Emby auto-discovers this because it implements IWebSocketListener
    public class WebSocketHandler : IWebSocketListener
    {
        private readonly IJsonSerializer _json;
        private readonly ILogger _logger;

        public string Name => "WebSocket";

        public WebSocketHandler(IJsonSerializer json, ILogger logger)
        {
            _json = json;
            _logger = logger;
        }

        public async Task ProcessMessage(WebSocketMessageInfo message)
        {
            _logger.Info("[WebSocket] Incoming raw: {0}", message.Data);

            JsonMessage msg;
            try
            {
                msg = _json.DeserializeFromString<JsonMessage>(message.Data);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("[WebSocket] Failed to parse JSON", ex);
                return;
            }

            if (msg?.Type == "ping")
            {
                _logger.Info("[WebSocket] Got PING from {0}", msg.UserId);

                var response = new JsonMessage
                {
                    Type = "pong",
                    UserId = msg.UserId,
                    Payload = "Server time: " + DateTime.UtcNow.ToString("HH:mm:ss")
                };

                var json = _json.SerializeToString(response);

                var reply = new WebSocketMessage<string>
                {
                    MessageType = Name,
                    Data = json
                };

                await message.Connection.SendAsync(reply, CancellationToken.None);
                _logger.Info("[WebSocket] Sent PONG back to client {0}", message.Connection.Id);
            }
        }
    }
}
