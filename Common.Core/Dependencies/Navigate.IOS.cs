#if __IOS__
using System;
using Foundation;
using UIKit;
using Common.Core;

[assembly: Xamarin.Forms.Dependency(typeof(Navigate))]
namespace Common.Core
{
    public class Navigate : INavigate
    {
        public void NavigateWithAddress(string address)
        {
            address = System.Net.WebUtility.UrlEncode(address);
            NSUrl mapUrl = NSUrl.FromString(string.Format("http://maps.apple.com/?daddr={0}", address));
            UIApplication.SharedApplication.OpenUrl(mapUrl);
        }
    }
}
#endif
