#if __IOS__
using System;
using BigTed;
using Common.Core;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ProgressIndicator))]
namespace Common.Core
{

    public class ProgressIndicator : IProgressIndicator
    {
        public static UIAlertController alert;
        public void ShowProgress(string message)
        {
            BTProgressHUD.Show(message);
        }

        public void Dismiss()
        {
            BTProgressHUD.Dismiss();
        }

        public void ShowProgress(string message, double percentage)
        {
            BTProgressHUD.Show(message, (float)percentage);
        }

    }
}
#endif

