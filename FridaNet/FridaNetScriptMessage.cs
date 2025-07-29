namespace FridaNet
{
    public enum FridaNetMessageType
    {
        Send,
        Error,
    }

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

    public delegate void FridaNetScriptMessageHandler(object sender, FridaNetScriptMessageEventArgs e);
}
