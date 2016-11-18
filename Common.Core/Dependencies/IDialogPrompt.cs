using System;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IDialogPrompt
    {
        void ShowMessage(string title, string message);
        void ShowMessage(string title, string message, Action callback);
        void ShowMessage(string title, string message, string[] buttonTitles, Action<bool> callBack);
        void ShowMessage(string title, string message, string[] buttonTitles, Action<int> callBack);
        void ShowActionSheet(string title, string subTitle, string[] list, Action<int> callBack);
        void ShowToast(string title, string message);
    }
}

