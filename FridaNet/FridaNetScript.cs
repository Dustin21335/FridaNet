using FridaScript = Frida.Script;

namespace FridaNet
{
    public class FridaNetScript
    {
        public FridaNetScript(FridaScript fridaScript)
        {
            FridaScript = fridaScript;
            FridaScript.Message += (s, e) => Message?.Invoke(s, new FridaNetScriptMessageEventArgs(e.Message, e.Data));
        }

        public FridaScript FridaScript { get; }

        public event FridaNetScriptMessageHandler? Message;

        public void Load()
        {
            FridaScript.Load();
        }

        public void Unload()
        {
            FridaScript.Unload();
        }

        public void Eternalize()
        {
            FridaScript.Eternalize();
        }

        public void PostWithData(string Message, byte[]? Data)
        {
            FridaScript.PostWithData(Message, Data);
        }

        public void Post(string Message)
        {
            PostWithData(Message, null);
        }

        public void EnableDebugger(ushort Port)
        {
            FridaScript.EnableDebugger(Port);
        }

        public void EnableDebugger()
        {
            EnableDebugger(0);
        }

        public void DisableDebugger()
        {
            FridaScript.DisableDebugger();
        }

        public void Dispose()
        {
            FridaScript.Dispose();
        }
    }
}
