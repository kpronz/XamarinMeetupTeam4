#if __ANDROID__
using System;
namespace Common.Core
{
    public class DeviceInfo : IDeviceInfo
    {
        public DeviceInformation GetDeviceInformation()
        {
            var di = new DeviceInformation();
            var serialNumber = Android.OS.Build.Serial;
            if (string.IsNullOrEmpty(serialNumber))
            {
                di.DeviceType = DeviceState.Simulator;
            }
            else {
                di.DeviceType = DeviceState.PhysicalDevice;
                di.SerialNumber = serialNumber;
                di.Model = Android.OS.Build.Model;
                di.Name = Android.OS.Build.Brand;
                di.OSVersion = Android.OS.Build.VERSION.Codename;
            }
            return di;
        }
    }
}
#endif

