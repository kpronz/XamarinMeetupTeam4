#if __ANDROID__
using System;
using Android.App;
using Common.Core;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DialogPrompt))]
namespace Common.Core
{
    public class DialogPrompt : IDialogPrompt
    {
        public void ShowMessage(string title, string message)
        {
            try
            {
                var dlg = new AlertDialog.Builder(Forms.Context).Create();
                dlg.SetTitle(title);
                dlg.SetMessage(message);
                dlg.SetCancelable(false);
                dlg.SetButton("Okay", (cs, ce) =>
                {

                });
                dlg.Show();

            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }
        }

        public void ShowMessage(string title, string message, string[] buttonTitles, Action<bool> callBack)
        {
            try
            {
                var d = new AlertDialog.Builder(Forms.Context);
                d.SetTitle(title);
                d.SetMessage(message);
                d.SetPositiveButton(buttonTitles[0], (e, a) =>
                {
                    callBack?.Invoke(true);
                });
                if (buttonTitles.Length > 1)
                {
                    d.SetNegativeButton(buttonTitles[1], (e, a) =>
                    {
                        callBack?.Invoke(false);
                    });
                }
                d.Create().Show();

            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }
        }

        public void ShowMessage(string title, string message, string[] buttonTitles, Action<int> callBack)
        {
            try
            {
                //var d1 = new AlertDialog.Builder(Forms.Context);

                //d1.SetPositiveButton("One", (sender, e) => { });
                //d1.SetNeutralButton("Two", (sender, e) => { });
                //d1.SetNegativeButton("Three", (sender, e) => { });
                //d1.Create().Show();

                var d = new AlertDialog.Builder(Forms.Context).Create();

                d.SetTitle(title);
                d.SetMessage(message);
                if (buttonTitles.Length > 2)
                {
                    d.SetButton(buttonTitles[0], (e, a) =>
                    {
                        callBack?.Invoke(0);
                    });
                    d.SetButton2(buttonTitles[1], (e, a) =>
                    {
                        callBack?.Invoke(1);
                    });
                    d.SetButton3(buttonTitles[2], (e, a) =>
                    {
                        callBack?.Invoke(2);
                    });


                }
                else {
                    d.SetButton(buttonTitles[0], (e, a) =>
                    {
                        callBack?.Invoke(0);
                    });
                    d.SetButton2(buttonTitles[1], (e, a) =>
                    {
                        callBack?.Invoke(1);
                    });
                }

                d.Show();

            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }
        }

        public void ShowActionSheet(string title, string subTitle, string[] list, Action<int> callBack)
        {
            var dlg = new AlertDialog.Builder(Forms.Context);
            dlg.SetTitle(title);
            dlg.SetSingleChoiceItems(list, -1, (s, a) =>
            {

                var index = list.IndexOf(list[a.Which]);
                callBack?.Invoke(index);
                ((AlertDialog)s).Dismiss();

            });
            dlg.SetPositiveButton("Cancel", (s, a) =>
            {

            });

            var dialog = dlg.Show();

        }

        public void ShowMessage(string title, string message, Action callback)
        {
            throw new NotImplementedException();
        }

        public void ShowCustomDialg(string title, string message, string button1, string button2, string button3, Action<int> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowToast(string title, string message)
        {
            throw new NotImplementedException();
        }
    }
}
#endif
