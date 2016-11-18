#if __ANDROID__
using Android.App;
using AndroidHUD;
using Common.Core;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ProgressIndicator))]
namespace Common.Core
{
    public class ProgressIndicator : IProgressIndicator
    {
        private static ProgressDialog dialog;

        public void ShowProgress(string message)
        {
            AndHUD.Shared.Show(Forms.Context, message, (int)MaskType.Clear);
        }

        public void Dismiss()
        {
            AndHUD.Shared.Dismiss(Forms.Context);
        }

        public void ShowProgress(string message, double percentage)
        {
            AndHUD.Shared.Show(Forms.Context, message, (int)percentage, MaskType.Clear);
        }
    }
}
#endif

