using System.Diagnostics;
using FridaProcess = Frida.Process;

namespace FridaNet
{
    public class FridaNetProcess : IDisposable
    {
        public FridaNetProcess(FridaProcess fridaProcess) 
        {
            FridaProcess = fridaProcess;
            IconDatas = FridaProcess.IconDatas;
            Parmameters = FridaProcess.Parameters.ToDictionary();
            Name = FridaProcess.Name;
            Pid = FridaProcess.Pid;
            Process = Process.GetProcessById((int)Pid);
        }

        public FridaProcess FridaProcess { get; }

        public Process Process { get; }

        public byte[][] IconDatas { get; }

        public Dictionary<string, object> Parmameters { get; }

        public string Name { get; }

        public uint Pid { get; }
        
        public void Kill()
        {
            try
            {
                Process.Kill();
                Process.WaitForExit();
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            FridaProcess.Dispose();
        }
    }
}
