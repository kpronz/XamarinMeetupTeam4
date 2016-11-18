using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Common.Core
{
    public class RelayCommand : ICommand, IDisposable
    {
        private Action<object> _execute;
        private Func<bool> _validator;
        private INotifyPropertyChanged _npc;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _validator != null ? _validator.Invoke() : true;
        }

        public RelayCommand(Action<object> execute, Func<bool> validator = null, INotifyPropertyChanged npc = null)
        {
            _execute = execute;
            _validator = validator;
            _npc = npc;

            if (_npc != null)
            {
                _npc.PropertyChanged += PropertyChangedEvent;
            }
        }
        private void PropertyChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            CanExecuteChanged?.Invoke(this, null);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        ~RelayCommand()
        {
            if (_npc != null)
                _npc.PropertyChanged -= PropertyChangedEvent;
        }
        public void Dispose()
        {
            if (_npc != null)
                _npc.PropertyChanged -= PropertyChangedEvent;
        }
    }

    public abstract class ObservableViewModel : ObservableObject, IDisposable
    {
        private bool isLoading;
        private string loadingMessage;

        public string LoadingMessage
        {
            get
            {
                return loadingMessage;
            }

            set
            {
                SetProperty(ref loadingMessage, value);
            }
        }

        public bool IsLoading
        {
            get
            {
                return isLoading;
            }

            set
            {
                SetProperty(ref isLoading, value);
                if (value)
                {
                    DependencyService.Get<IProgressIndicator>().ShowProgress(loadingMessage);
                }
                else
                {
                    DependencyService.Get<IProgressIndicator>().Dismiss();
                }
            }
        }

        protected void ShowMessage(string title, string message)
        {
            DependencyService.Get<IDialogPrompt>().ShowMessage(title, message);
        }

        protected void ShowOkMessage(string title, string message, Action<bool> callback)
        {
            DependencyService.Get<IDialogPrompt>().ShowMessage(title, message, new string[]
            {
                "OK"
            }, callback);
        }

        protected void ShowMessage(string title, string message, Action<bool> callback)
        {
            DependencyService.Get<IDialogPrompt>().ShowMessage(title, message, new string[]
            {
                "OK", "Cancel"
            }, callback);
        }



        protected void ShowActionSheet(string title, string message, string[] options, Action<int> callback)
        {
            DependencyService.Get<IDialogPrompt>().ShowActionSheet(title, message, options, callback);
        }

        protected void ShowFinished(string title, string message, Action<bool> callback)
        {
            DependencyService.Get<IDialogPrompt>().ShowMessage(title, message, new string[]
            {
                "OK"
            }, callback);
        }

        ///// <summary>
        ///// Message received from another view model instance
        ///// </summary>
        ///// <param name="key">Key.</param>
        ///// <param name="obj">Object.</param>
        //public virtual void OnInternalMesssageReceived(string key, object obj) { }

        /// <summary>
        /// Broadcast message to all view model instances
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="obj">Object.</param>
        protected void SendViewMessageAll(string key, object obj)
        {
            foreach (var vmName in AppData.ViewModels.Keys)
            {
                ((ObservableViewModel)AppData.ViewModels[vmName]).OnViewMessageReceived(key, obj);
            }
        }
        /// <summary>
        /// Broadcast message to a particular view model instance
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected void SendViewMessage<T>(string key, object obj) where T : ObservableViewModel, new()
        {
            T vm = null;
            var n = typeof(T).Name;
            if (!AppData.ViewModels.ContainsKey(n))
            {
                vm = new T();
                AppData.ViewModels.Add(n, vm);
            }
            else {
                vm = (T)AppData.ViewModels[n];
            }
            vm?.OnViewMessageReceived(key, obj);
        }

        protected bool IsEmtpyOrNull(params string[] properties)
        {
            foreach (var prop in properties)
            {
                if (string.IsNullOrEmpty(prop))
                    return true;
            }
            return false;
        }
        public virtual void Dispose() { }
        public abstract void OnInit();
        protected abstract void OnViewMessageReceived(string key, object obj);
    }
}

