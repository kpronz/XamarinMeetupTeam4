using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Net;
using Xamarin.Auth;

namespace Common.Core
{
    public class AppVersion
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public DateTime VersionDate { get; set; }
    }
    public class AppData
    {
        public static AuthenticationToken TokenBearer { get; set; }
        public static NetworkCredential HttpCredentials { get; set; }
        public static int HttpTimeOut { get; set; }
        public static bool HttpAllowAutoRedirect { get; set; }

        public static Dictionary<string, ObservableViewModel> ViewModels { get; set; }
        public static string VMDictionary = "VMDictionary";

        public static bool IsConnected { get; set; }
        public static INavigation AppNav { get { return Application.Current.MainPage.Navigation; } }


        public static bool TokenIsValid
        {
            get
            {
                return TokenBearer?.expires > DateTimeOffset.Now;
            }
        }

        public static T GetViewModel<T>() where T : ObservableViewModel, new()
        {
            if (ViewModels == null) ViewModels = new Dictionary<string, ObservableViewModel>();

            var name = typeof(T).Name;
            if (ViewModels.ContainsKey(name))
            {
                try
                {
                    return (T)ViewModels[name];
                }
                catch
                {
                    ViewModels[name] = new T();
                    FileStore.DeleteAsync(AppData.VMDictionary).ContinueOn();
                    return (T)ViewModels[name];
                }
            }
            else
            {
                var obj = new T();
                obj.OnInit();
                ViewModels.Add(name, obj);
                return obj;
            }
        }

        public static void SetViewModel<T>(T obj) where T : ObservableViewModel
        {
            if (ViewModels == null) ViewModels = new Dictionary<string, ObservableViewModel>();

            var name = typeof(T).Name;
            if (ViewModels.ContainsKey(name))
            {
                ViewModels[name] = obj;
            }
            else
            {
                ViewModels.Add(name, obj);
            }
        }

        public static async Task ResetViewModels()
        {
            await Task.Run(async () =>
            {
                await FileStore.DeleteAsync(AppData.VMDictionary);
                var keys = ViewModels.Keys.ToArray();
                foreach (var key in keys)
                {
                    ViewModels[key].Dispose();
                }
                ViewModels.Clear();
            });
        }

        public static void LoadPersistedViewModels()
        {
            if (AppData.ViewModels == null || AppData.ViewModels.Keys.Count == 0)
            {
                FileStore.GetAsync<Dictionary<string, ObservableViewModel>>(AppData.VMDictionary).ContinueWith((data) =>
                {
                    var result = data.Result;
                    if (result.Success)
                    {
                        AppData.ViewModels = result.Response;
                    }
                    else
                    {
                        AppData.ViewModels = new Dictionary<string, ObservableViewModel>();
                    }
                });
            }
        }
        public static void PersistViewModels()
        {
            if (AppData.ViewModels != null)
            {
                FileStore.SaveAsync<Dictionary<string, ObservableViewModel>>(AppData.VMDictionary, AppData.ViewModels).ContinueWith((data) =>
                {
                    var result = data.Result.Success;
                });
            }
        }
    }
}

