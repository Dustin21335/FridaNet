namespace FridaNet
{
    public class FridaNetScriptMessageEventArgs : EventArgs
    {
        public string Message { get; }

        public byte[] Data { get; }

        public FridaNetMessageType MessageType { get; }

        public string Payload { get; }

        public FridaNetScriptMessageEventArgs(string message, byte[] data, FridaNetMessageType messageType, string payload)
        {
            Message = message;
            Data = data;
            MessageType = messageType;
            Payload = payload;
        }
    }

    public enum FridaNetMessageType
    {
        Send,
        Error,
        Log,
    }

    public delegate void FridaNetScriptMessageHandler(object sender, FridaNetScriptMessageEventArgs e);
}
