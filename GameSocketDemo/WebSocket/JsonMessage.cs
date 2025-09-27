namespace GameStreamEngine.WebSocket
{
    public class JsonMessage
    {
        public string Type { get; set; }
        public string Payload { get; set; }
        public string UserId { get; set; }
        public string GameId { get; set; }
    }
}

