namespace SmartMass.Controller.Api.Services
{
    public interface IDiscoveredDevices
    {
        void Add(string deviceId);
        void Remove(string deviceId);
        IEnumerable<string> GetAll();
        bool Contains(string deviceId);
    }

    public class DiscoveredDevices : IDiscoveredDevices
    {
        private List<string> devices = new List<string>();

        public void Add(string deviceId)
        {
            if (!Contains(deviceId)) devices.Add(deviceId);
        }

        public void Remove(string deviceId)
        {
            if (Contains(deviceId)) devices.Remove(deviceId);
        }

        public IEnumerable<string> GetAll()
        {
            return devices;
        }

        public bool Contains(string deviceId)
        {
            return devices.Contains(deviceId);
        }
    }
}
