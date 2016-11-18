using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

#if __IOS__
using UIKit;
#endif
namespace Common.Core
{
    public class EncryptedStore
    {
        public static string SerialNumber
        {
            get
            {


#if __ANDROID__
                return !string.IsNullOrEmpty(Android.OS.Build.Serial) ? "432!&4bm-ms@02XX)7)" : Android.OS.Build.Serial;
#endif
#if __IOS__

                if (ObjCRuntime.Runtime.Arch.ToString() == "SIMULATOR")
                    return "432!&4bm-ms@02XX)7)";
                else
                    return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
#endif
            }
        }

        public static async Task<GenericResponse<T>> GetAccount<T>(string fileName) where T : class, new()
        {
            return await Task.Run(async () =>
            {
                var response = new GenericResponse<T>() { Success = false };
                try
                {

                    var eDataString = await FileStore.GetStringAsync(fileName);
                    if (eDataString.Success)
                    {
                        var dataString = Crypto.Decrypt(eDataString.Response, SerialNumber);
                        response.Response = JsonConvert.DeserializeObject<T>(dataString);
                        response.Success = true;
                    }
                    else {
                        response.Error = eDataString.Error;
                    }

                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                return response;
            });
        }

        public static async Task<GenericResponse<T>> GetAccount<T>(string fileName, string password) where T : class, new()
        {
            return await Task.Run(async () =>
            {
                var response = new GenericResponse<T>() { Success = false };
                try
                {

                    var eDataString = await FileStore.GetStringAsync(fileName);
                    if (eDataString.Success)
                    {
                        var dataString = Crypto.Decrypt(eDataString.Response, $"{SerialNumber}+{password}");
                        response.Response = JsonConvert.DeserializeObject<T>(dataString);
                        response.Success = true;
                    }
                    else
                    {
                        response.Error = eDataString.Error;
                    }

                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                return response;
            });
        }

        public static async Task<BooleanResponse> SaveAccount<T>(string fileName, T obj) where T : class, new()
        {
            return await Task.Run(async () =>
            {
                var response = new BooleanResponse();
                try
                {

                    var dataString = JsonConvert.SerializeObject(obj);
                    var eDataString = Crypto.Encrypt(dataString, SerialNumber);
                    response = await FileStore.SaveStringAsync(fileName, eDataString);
                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                return response;
            });

        }

        public static async Task<BooleanResponse> SaveAccount<T>(string fileName, string password, T obj) where T : class, new()
        {
            return await Task.Run(async () =>
            {
                var response = new BooleanResponse();
                try
                {
                    var dataString = JsonConvert.SerializeObject(obj);
                    var eDataString = Crypto.Encrypt(dataString, $"{SerialNumber}+{password}");
                    response = await FileStore.SaveStringAsync(fileName, eDataString);
                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                return response;
            });

        }

        public static async Task<BooleanResponse> DeleteFileStore(string fileName)
        {
            var response = new BooleanResponse();
            try
            {
                await FileStore.DeleteAsync(fileName);
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }
    }
}
