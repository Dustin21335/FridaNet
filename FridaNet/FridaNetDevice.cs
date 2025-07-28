using Frida;
using FridaDevice = Frida.Device;

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

        public FridaNetSession? Attach(FridaNetProcess? fridaNetProcess)
        { 
            return fridaNetProcess != null ? new FridaNetSession(FridaDevice.Attach(fridaNetProcess.Pid)) : null;
        }

        public List<FridaNetProcess> GetProcesses(uint[]? pids, Scopes scope)
        {
            return FridaDevice.EnumerateProcesses(pids, (Scope)scope).Select(p => new FridaNetProcess(p)).ToList();
        }

        public List<FridaNetProcess> GetProcesses(Scopes scope)
        {
            return GetProcesses(null, scope);
        }

        public FridaNetProcess? GetProcess(string Name)
        {
            return GetProcesses(Scopes.Full).FirstOrDefault(p => p.Name.Trim().ToLower() == Name.Trim().ToLower());
        }

        public FridaNetProcess? GetProcess(uint Pid)
        {
            return GetProcesses(Scopes.Full).FirstOrDefault(p => p.Pid == Pid);
        }

        public void Dispose()
        {
            FridaDevice.Dispose();
        }
    }
}
