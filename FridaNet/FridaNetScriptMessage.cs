namespace FridaNet
{
    public class FridaNetScriptMessageEventArgs : EventArgs
    {
        public string Message { get; }


        public byte[] Data { get; }

        public FridaNetScriptMessageEventArgs(string message, byte[] data)
        {
            Message = message;
            Data = data;
        }
    }

    public delegate void FridaNetScriptMessageHandler(object sender, FridaNetScriptMessageEventArgs e);
}
