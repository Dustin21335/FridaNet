using FridaSession = Frida.Session;

namespace FridaNet
{
    public class FridaNetSession : IDisposable
    {
        public FridaNetSession(FridaSession fridaSession, FridaNetProcess fridaNetProcess)
        {
            FridaSession = fridaSession;
            Pid = FridaSession.Pid;
            FridaNetProcess = fridaNetProcess;
            FridaSession.Detached += (s, e) => Detached?.Invoke(s, new FridaNetSessionDetachedEventArgs((FridaNetSessionDetachReason)e.Reason));
        }

        public FridaSession FridaSession { get; }

        public uint Pid { get; }

        public FridaNetProcess FridaNetProcess { get; }

        public event FridaNetSessionDetachedHandler? Detached;

        public void Detach()
        {
            FridaSession.Detach();
        }

        public FridaNetScript CreateScript(string Source, string? Name)
        {
            return new FridaNetScript(FridaSession.CreateScript(Source, Name));
        }

        public FridaNetScript CreateScript(string Source)
        {
            return CreateScript(Source, null);
        }

        public void Dispose()
        {
            FridaSession.Dispose();
        }
    }
}
