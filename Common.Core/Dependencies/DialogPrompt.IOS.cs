#if __IOS__
using System;
using System.Threading.Tasks;
using BigTed;
using Common.Core;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(DialogPrompt))]
namespace Common.Core
{
    public class DialogPrompt : IDialogPrompt
    {

        public void ShowMessage(string title, string message, string[] buttonTitle, Action<bool> callBack)
        {
            var controller = GetUIController();
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create(buttonTitle[0], UIAlertActionStyle.Default, (action) =>
           {
               callBack?.Invoke(true);
           }));
            if (buttonTitle.Length > 1)
            {
                alert.AddAction(UIAlertAction.Create(buttonTitle[1], UIAlertActionStyle.Default, (action) =>
                {
                    callBack?.Invoke(false);
                }));
            }

            controller.PresentViewController(alert, true, null);
        }

        public void ShowMessage(string title, string message, string[] buttonTitles, Action<int> callBack)
        {
            var controller = GetUIController();
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            foreach (var txt in buttonTitles)
            {
                alert.AddAction(UIAlertAction.Create(txt, UIAlertActionStyle.Default, action =>
                {
                    callBack?.Invoke(buttonTitles.IndexOf(txt));
                }));
            }

            controller.PresentViewController(alert, true, null);
        }

        public void ShowMessage(string title, string message)
        {
            var controller = GetUIController();
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            controller.PresentViewController(alert, true, null);
        }

        public void ShowMessage(string title, string message, Action callback)
        {
            var controller = GetUIController();
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, action =>
            {
                callback?.Invoke();
            }));
            controller.PresentViewController(alert, true, null);
        }

        public void ShowActionSheet(string title, string subTitle, string[] list, Action<int> callBack)
        {
            var controller = GetUIController();
            var alert = UIAlertController.Create(title, subTitle, UIAlertControllerStyle.ActionSheet);
            alert.View.TintColor = UIColor.Black;
            foreach (var obj in list)
            {
                alert.AddAction(UIAlertAction.Create(obj, UIAlertActionStyle.Default, (action) =>
                {
                    var index = list.ToList().IndexOf(action.Title);
                    callBack.Invoke(index);
                }));
            }
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) =>
            {
                callBack.Invoke(-1);
            }));

            var presentationPopover = alert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = controller.View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                presentationPopover.SourceRect = controller.View.Frame;
            }
            controller.PresentViewController(alert, true, null);
        }

        private UIViewController GetUIController()
        {
            var win = UIApplication.SharedApplication.KeyWindow;
            var vc = win.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }

        public void ShowCustomDialg(string title, string message, string button1, string button2, string button3, Action<int> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowToast(string title, string message)
        {
            BTProgressHUD.ShowToast("Hello from Toast", false, 4000);

        }
    }
}
#endif

