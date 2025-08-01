using System.Text.Json;
using FridaScript = Frida.Script;

namespace FridaNet
{
    public class FridaNetScript
    {
        public FridaNetScript(FridaScript fridaScript)
        {
            FridaScript = fridaScript;
            FridaScript.Message += (s, e) =>
            {
                try
                {
                    JsonElement json = JsonDocument.Parse(e.Message).RootElement;
                    if (!Enum.TryParse(json.TryGetProperty("type", out JsonElement type) && type.ValueKind == JsonValueKind.String ? type.GetString() : "", true, out FridaNetMessageType messageType)) messageType = FridaNetMessageType.Send;
                    Message?.Invoke(s, new FridaNetScriptMessageEventArgs(e.Message, e.Data ?? Array.Empty<byte>(), messageType, json.TryGetProperty("payload", out JsonElement payload) ? payload.GetRawText() : ""));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to parse message {e.Message}: {ex.Message}");
                }
            };
        }

        public FridaScript FridaScript { get; }

        public event FridaNetScriptMessageHandler? Message;

        public bool ScriptLoaded { get; private set; }

        public void Load()
        {
            ScriptLoaded = true;
            FridaScript.Load();
        }

        public void Unload()
        {
            ScriptLoaded = false;
            FridaScript.Unload();
        }

        public void Toggle()
        {
            if (ScriptLoaded) Unload();
            else Load();
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

        public void PostWithData(FridaNetMessageType fridaNetMessageType, string payload, byte[]? data)
        {
            FridaScript?.PostWithData(JsonSerializer.Serialize(new Dictionary<string, object>
            {
                ["type"] = fridaNetMessageType.ToString().ToLower(),
                ["payload"] = payload
            }), data);        
        }

        public void Post(FridaNetMessageType fridaNetMessageType, string payload)
        {
            PostWithData(fridaNetMessageType, payload, null);
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
