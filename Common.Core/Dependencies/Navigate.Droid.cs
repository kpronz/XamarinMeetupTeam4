#if __ANDROID__
using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Common.Core;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Navigate))]
namespace Common.Core
{
    public class Navigate : INavigate
    {
        public void NavigateWithAddress(string address)
        {
            try
            {
                var activity = (Activity)Xamarin.Forms.Forms.Context;
                address = System.Net.WebUtility.UrlEncode(address);
                var gmmIntentUri = Android.Net.Uri.Parse("google.navigation:q=" + address);
                var mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
                mapIntent.SetPackage("com.google.android.apps.maps");
                activity.StartActivity(mapIntent);
            }
            catch (Exception ex)
            {
                Toast toast = Toast.MakeText(Xamarin.Forms.Forms.Context, "This activity is not supported", ToastLength.Long);
                toast.Show();
            }

        }
    }
}
#endif
