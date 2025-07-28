using FridaDeviceManager = Frida.DeviceManager;

namespace FridaNet
{
    public class FridaNetDeviceManager : IDisposable
    {
        public FridaNetDeviceManager() 
        {
            FridaDeviceManager = new FridaDeviceManager();
            FridaDeviceManager.Changed += (s, e) => Changed?.Invoke(s, e);
        }

        public FridaDeviceManager FridaDeviceManager { get; }

        public event EventHandler? Changed;

        public List<FridaNetDevice> GetDevices()
        {
            return FridaDeviceManager.EnumerateDevices().Select(d => new FridaNetDevice(d)).ToList();  
        }

        public FridaNetDevice? GetDeviceByName(string Name)
        {
            return GetDevices().FirstOrDefault(d => d.Name.Trim().ToLower() == Name.Trim().ToLower());
        }

        public FridaNetDevice? GetDeviceById(string id)
        {
            return GetDevices().FirstOrDefault(d => d.Id.Trim().ToLower() == id.Trim().ToLower());
        }

        public FridaNetDevice? GetLocalDevice()
        {
            return GetDeviceById("local");
        }

        public void Dispose()
        {
            FridaDeviceManager.Dispose();
        }
    }
}
