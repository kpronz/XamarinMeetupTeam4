#if __IOS__
using System;
using UIKit;

namespace Common.Core
{
    public class DeviceInfo : IDeviceInfo
    {
        public DeviceInformation GetDeviceInformation()
        {
            var di = new DeviceInformation();
            if (ObjCRuntime.Runtime.Arch.ToString() == "SIMULATOR")
            {
                di.DeviceType = DeviceState.Simulator;
            }
            else {
                di.Model = UIDevice.CurrentDevice.Model;
                di.Name = UIDevice.CurrentDevice.Name;
                di.OSVersion = UIDevice.CurrentDevice.SystemName;
                di.DeviceType = DeviceState.PhysicalDevice;
                di.SerialNumber = UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            }

            return di;
        }
    }
}
#endif

