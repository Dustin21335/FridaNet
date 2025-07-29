using Frida;
using FridaDevice = Frida.Device;
using Process = System.Diagnostics.Process;

namespace FridaNet
{
    public class FridaNetDevice : IDisposable
    {
        public FridaNetDevice(FridaDevice fridaDevice)
        {
            FridaDevice = fridaDevice;
            Name = FridaDevice.Name;
            Id = FridaDevice.Id;
            DeviceType = (DeviceTypes)FridaDevice.Type;
            FridaDevice.Lost += (s, e) => Lost?.Invoke(s, e);
            IconData = FridaDevice.IconData;
        }

        public enum DeviceTypes
        {
            Local,
            Remote,
            Usb
        }

        public enum Scopes
        {
            Minimal,
            Metadata,
            Full
        }

        public FridaDevice FridaDevice { get; }

        public string Name { get; }

        public string Id { get; }

        public DeviceTypes DeviceType { get; }

        public byte[] IconData { get; }

        public event EventHandler? Lost;

        public FridaNetSession? Attach(string name)
        {
            return Attach(GetProcess(name));
        }

        public FridaNetSession? Attach(uint pid)
        {
            return Attach(GetProcess(pid));
        }

        public FridaNetSession? Attach(Process process)
        {
            return Attach(GetProcess(process));
        }

        public FridaNetSession? Attach(FridaNetProcess? fridaNetProcess)
        { 
            return fridaNetProcess != null ? new FridaNetSession(FridaDevice.Attach(fridaNetProcess.Pid), fridaNetProcess) : null;
        }

        public void Resume(string name)
        {
            Resume(GetProcess(name));
        }

        public void Resume(uint pid)
        {
            Resume(GetProcess(pid));
        }

        public void Resume(Process process)
        {
            Resume(GetProcess(process));
        }

        public void Resume(FridaNetProcess? fridaNetProcess)
        {
            if (fridaNetProcess != null) FridaDevice.Resume(fridaNetProcess.Pid);
        }

        public List<FridaNetProcess> GetProcessesByPids(uint[]? pids, Scopes scope)
        {
            return FridaDevice.EnumerateProcesses(pids, (Scope)scope).Select(p => new FridaNetProcess(p)).ToList();
        }

        public List<FridaNetProcess> GetProcessesByProcesses(Process[]? processes, Scopes scope)
        {
            return FridaDevice.EnumerateProcesses(processes?.Select(p => (uint)p.Id).ToArray(), (Scope)scope).Select(p => new FridaNetProcess(p)).ToList();
        }

        public List<FridaNetProcess> GetProcesses(Scopes scope)
        {
            return GetProcessesByPids(null, scope);
        }

        public FridaNetProcess? GetProcess(string Name)
        {
            return GetProcesses(Scopes.Full).FirstOrDefault(p => p.Name.Trim().ToLower() == Name.Trim().ToLower());
        }

        public FridaNetProcess? GetProcess(uint Pid)
        {
            return GetProcesses(Scopes.Full).FirstOrDefault(p => p.Pid == Pid);
        }

        public FridaNetProcess? GetProcess(Process process)
        {
            return GetProcesses(Scopes.Full).FirstOrDefault(p => p.Pid == process.Id);
        }

        public uint SpawnProcessUint(string processPath, string[]? argv, string[]? envp, string[]? env, string? cwd)
        {
            return FridaDevice.Spawn(processPath.Trim(), argv, envp, env, cwd);
        }

        public uint SpawnProcessUint(string processPath)
        {
            return SpawnProcessUint(processPath, null, null, null, null);
        }

        public FridaNetProcess? SpawnProcess(string processPath, string[]? argv, string[]? envp, string[]? env, string? cwd)
        {
            return GetProcess(SpawnProcessUint(processPath.Trim(), argv, envp, env, cwd));
        }

        public FridaNetProcess? SpawnProcess(string processPath)
        {
            return SpawnProcess(processPath, null, null, null, null);
        }

        public void Dispose()
        {
            FridaDevice.Dispose();
        }
    }
}
