using System;
namespace Common.Core
{
    public enum DeviceState
    {
        Simulator,
        PhysicalDevice
    }
    public class DeviceInformation
    {
        public DeviceState DeviceType { get; set; }
        public string OSVersion { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
    }
    public interface IDeviceInfo
    {
        DeviceInformation GetDeviceInformation();
    }
}

