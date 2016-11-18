using System;
using Xamarin.Forms;

namespace Common.Core
{
    public abstract class BoundPage<T> : ContentPage, IDisposable
        where T : ObservableViewModel, new()
    {
        public string HeaderTitle { get; set; }
        public T VM
        {
            get { return AppData.GetViewModel<T>(); }
            set { AppData.SetViewModel<T>(value); }
        }
        public BoundPage()
        {
            this.BindingContext = VM;
        }

        public void Dispose()
        {

        }

    }
}

