using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.Core
{
    public class AuthenticationToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public Dictionary<string, string> meta_data { get; set; }
        public DateTime expires { get; set; }
        public AuthenticationToken()
        {

        }
        public AuthenticationToken(string jsonSerializedData)
        {
            var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonSerializedData);
            this.access_token = (string)token["access_token"];
            this.token_type = (string)token["token_type"];
            this.expires_in = int.Parse(token["expires_in"].ToString());
            this.meta_data = JsonConvert.DeserializeObject<Dictionary<string, string>>((string)token["UserDetail"]);
            this.expires = DateTime.Now.AddSeconds(expires_in);

        }

    }
    public class WebDownloadClient : INotifyPropertyChanged
    {
        private double progress;
        public string DownloadUrl { get; set; }
        public WebClient Client { get; set; }
        public Action<byte[]> FinishedEvent { get; set; }
        public Action<double> PercentageChanged { get; set; }

        public double Progress
        {
            get
            {
                return progress;
            }

            set
            {
                PercentageChanged?.Invoke(value);
                SetProperty(ref progress, value);
            }
        }
        public async Task StartDownload()
        {
            if (Client == null)
                Client = new WebClient();

            await Task.Run(() =>
            {
                Client.DownloadProgressChanged += DownprogressChanged;
                Client.DownloadDataCompleted += DownloadComplete;
                Client.DownloadDataAsync(new Uri(DownloadUrl));
            });

        }
        public void UnhookEvents()
        {
            Client.DownloadProgressChanged -= DownprogressChanged;
            Client.DownloadDataCompleted -= DownloadComplete;
        }
        private void DownloadComplete(object sender, DownloadDataCompletedEventArgs args)
        {
            FinishedEvent?.Invoke(args.Result);
        }
        private void DownprogressChanged(object sender, DownloadProgressChangedEventArgs args)
        {
            Progress = (float)args.BytesReceived / (float)args.TotalBytesToReceive;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;

            if (onChanged != null)
                onChanged();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
        public void Notify(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
    public class HttpService
    {
        public static string json;

        public static WebClient GetWebClient()
        {
            var client = new WebClient();
            if (AppData.TokenBearer != null)
                client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + AppData.TokenBearer.access_token);
            return client;
        }

        private static HttpClient GetClient()
        {
            HttpClient client = null;
#if __IOS__
            var handler = new HttpClientHandler();
#elif __ANDROID__
            var handler = new HttpClientHandler();
#else
			var handler = new HttpClientHandler();
#endif

            handler.AllowAutoRedirect = AppData.HttpAllowAutoRedirect;

            if (AppData.HttpCredentials != null)
                handler.Credentials = AppData.HttpCredentials;

            if (AppData.HttpTimeOut > 0)
            {
                client = new HttpClient(handler, true) { Timeout = new TimeSpan(0, 0, AppData.HttpTimeOut) };
            }
            else
            {
                client = new HttpClient(handler, true);
            }

            if (AppData.TokenBearer != null)
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + AppData.TokenBearer.access_token);

            return client;
        }
        public static async Task<StringResponse> FormPost(string url, HttpContent content)
        {
            var response = new StringResponse() { };
            try
            {
                using (var client = GetClient())
                {
                    var postResponse = await client.PostAsync(url, content);
                    postResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    postResponse.EnsureSuccessStatusCode();

                    var raw = await postResponse.Content.ReadAsStringAsync();
                    if (raw != null)
                    {
                        response.Response = raw;
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                response.Error = ex;
            }
            return response;
        }

        public static async Task<GenericResponse<T>> Get<T>(string url) where T : class, new()
        {
            var response = new GenericResponse<T>() { };
            try
            {
                using (var client = GetClient())
                {
                    using (var srvResponse = client.GetAsync(url).Result)
                    {
                        json = await GetStringContent<T>(srvResponse);
                        response.Response = await DeserializeObject<T>(json);
                        response.Success = true;
                        json = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                response.Error = ex;
                response.MetaData = json;
            }

            return response;
        }
        public static async Task<GenericResponse<T>> Post<T>(string url, object obj) where T : class, new()
        {
            var response = new GenericResponse<T>() { };
            try
            {
                using (var client = GetClient())
                {
                    var data = JsonConvert.SerializeObject(obj);
                    using (var srvResponse = client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).Result)
                    {
                        json = await GetStringContent<T>(srvResponse);
                        response.Response = await DeserializeObject<T>(json);
                        response.Success = true;
                        json = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                response.Error = ex;
                response.MetaData = json;
            }


            return response;

        }
        public static async Task<GenericResponse<T>> Put<T>(string url, object obj) where T : class, new()
        {
            var response = new GenericResponse<T>() { };
            try
            {
                using (var client = GetClient())
                {
                    var data = JsonConvert.SerializeObject(obj);
                    using (var srvResponse = client.PutAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).Result)
                    {
                        json = await GetStringContent<T>(srvResponse);
                        response.Response = await DeserializeObject<T>(json);
                        response.Success = true;
                        json = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                response.Error = ex;
                response.MetaData = json;
            }

            return response;
        }

        public static async Task<string> GetStringContent<T>(HttpResponseMessage response) where T : class, new()
        {
            var jsonResult = await response.Content.ReadAsStringAsync();
#if DEBUG
            Console.WriteLine();
            Console.WriteLine();
            var name = typeof(T).Name;
            if (name == "List`1")
            {
                var types = typeof(T).GetGenericArguments();
                if (types != null && types.Length > 0)
                {
                    var obj = types[0];
                    name = "Collection of " + obj.Name;
                }

            }
            Console.WriteLine($"*-*-*-*-*-*-*-*-*-*-*-*- {name} - HTTP STRING RESULT *-*-*-*-*-*-*-*-*-*-*-*-*-");
            var formatted = await FormattedJson(jsonResult);
            Console.WriteLine(formatted);
            Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
            Console.WriteLine();
            Console.WriteLine();
#endif
            return jsonResult;
        }

        private static Task<string> FormattedJson(string jsonResult)
        {
            return Task.Run(() =>
            {
                var obj = JsonConvert.DeserializeObject(jsonResult);
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            });
        }

        private static Task<T> DeserializeObject<T>(string content) where T : class, new()
        {
            return Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<T>(content);
            });
        }

    }

    public static class JsonDataExtension
    {
        public static T ConvertTo<T>(this StringResponse str) where T : struct
        {
            object result = null;
            var code = Type.GetTypeCode(typeof(T));
            switch (code)
            {
                case TypeCode.Int32:
                    result = JsonConvert.DeserializeObject<int>(str.Response);
                    break;
                case TypeCode.Int16:
                    result = JsonConvert.DeserializeObject<short>(str.Response);
                    break;
                case TypeCode.Int64:
                    result = JsonConvert.DeserializeObject<long>(str.Response);
                    break;
                case TypeCode.String:
                    result = JsonConvert.DeserializeObject<string>(str.Response);
                    break;
                case TypeCode.Boolean:
                    result = JsonConvert.DeserializeObject<bool>(str.Response);
                    break;
                case TypeCode.Double:
                    result = JsonConvert.DeserializeObject<double>(str.Response);
                    break;
                case TypeCode.Decimal:
                    result = JsonConvert.DeserializeObject<decimal>(str.Response);
                    break;
                case TypeCode.Byte:
                    result = JsonConvert.DeserializeObject<Byte>(str.Response);
                    break;
                case TypeCode.DateTime:
                    result = JsonConvert.DeserializeObject<DateTime>(str.Response);
                    break;
                case TypeCode.Single:
                    result = JsonConvert.DeserializeObject<Single>(str.Response);
                    break;
            }
            return (T)result;
        }
    }
}

