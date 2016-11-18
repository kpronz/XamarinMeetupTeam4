using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Auth;

namespace Common.Core
{
    public class AccountService
    {
        const string unprotectedStore = "unprotectedStoreTokenAccount";
        const string protectedStore = "protectedStoreTokenAccount";
        const string pwKey = "password";

        public static async Task<BooleanResponse> SaveToAccountStore<T>(string username, T obj) where T : class, new()
        {
            return await Task.Run(() =>
            {
                var response = new BooleanResponse() { Success = false };
                try
                {
                    var account = GetAccount(username, false);
                    PersistAccount(account, username, null, obj);
                    SaveAccount(account, username, false);
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                return response;
            });

        }
        public static async Task<GenericResponse<T>> GetFromAccountStore<T>(string username) where T : class, new()
        {
            return await Task.Run(() =>
            {
                var response = new GenericResponse<T>() { Success = false };
                try
                {
                    var account = GetAccount(username, false);
                    response.Response = LoadAccount<T>(account, username);
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                return response;

            });

        }
        public static async Task<BooleanResponse> SaveToAccountStore<T>(string username, string password, T obj) where T : class, new()
        {
            return await Task.Run(() =>
            {
                var response = new BooleanResponse() { Success = false };
                try
                {
                    var account = GetAccount(username, true);
                    PersistAccount(account, username, password, obj);
                    SaveAccount(account, username, true);
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                return response;
            });

        }
        public static async Task<GenericResponse<T>> GetFromAccountStore<T>(string username, string password) where T : class, new()
        {
            return await Task.Run(() =>
            {
                var response = new GenericResponse<T>() { Success = false };
                try
                {
                    var account = GetAccount(username, true);
                    response.Response = LoadAccount<T>(account, password);
                    if (response.Response != null)
                        response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                return response;

            });

        }

        private static void PersistAccount<T>(Account account, string username, string password, T obj) where T : class, new()
        {
            var data = JsonConvert.SerializeObject(obj);
            if (account.Properties.ContainsKey(typeof(T).Name))
            {
                if (!string.IsNullOrEmpty(password))
                {
                    account.Properties[pwKey] = Crypto.HashPassword(password);
                    account.Properties[typeof(T).Name] = Crypto.Encrypt(data, password);
                }
                else {
                    account.Properties[typeof(T).Name] = data;
                }

            }
            else {
                if (!string.IsNullOrEmpty(password))
                {
                    account.Properties.Add(pwKey, Crypto.HashPassword(password));
                    account.Properties.Add(typeof(T).Name, Crypto.Encrypt(data, password));
                }
                else {
                    account.Properties.Add(typeof(T).Name, data);
                }

            }
        }
        private static T LoadAccount<T>(Account account, string password) where T : class, new()
        {
            if (!string.IsNullOrEmpty(password))
            {
                var hashedPassword = Crypto.HashPassword(password);
                if (account.Properties.ContainsKey(typeof(T).Name) && account.Properties[pwKey] == hashedPassword)
                {
                    var data = Crypto.Decrypt(account.Properties[typeof(T).Name], password);
                    return JsonConvert.DeserializeObject<T>(data);
                }

            }
            else {
                var data = account.Properties[typeof(T).Name];
                return JsonConvert.DeserializeObject<T>(data);
            }
            return null;

        }



        private static AccountStore GetStore()
        {
#if __ANDROID__
            return AccountStore.Create(Android.App.Application.Context);
#else
            return AccountStore.Create();
#endif
        }
        private static void SaveAccount(Account account, string username, bool protectedAccount)
        {
            if (username == null)
                return;

            var store = GetStore();
            var serviceId = protectedAccount == true ? protectedStore : unprotectedStore;
            store.Save(account, serviceId);
        }
        private static Account GetAccount(string username, bool protectedAccount)
        {
            if (username == null)
                return null;

            var store = GetStore();
            var serviceId = protectedAccount == true ? protectedStore : unprotectedStore;
            var accounts = store.FindAccountsForService(serviceId);
            var storeAccount = accounts.FirstOrDefault(a => a.Username == username);
            if (storeAccount == null)
            {
                storeAccount = new Account(username);
            }
            return storeAccount;
        }

    }
}

