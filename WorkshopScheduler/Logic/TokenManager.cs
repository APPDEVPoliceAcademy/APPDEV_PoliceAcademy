using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkshopScheduler.Models;
using Xamarin.Auth;

namespace WorkshopScheduler.Logic
{
    public static class TokenManager
    {

        private const string UserName = "Local";

        public static bool SaveToken(TokenInfo token)
        {
            if(token == null) return false;
            var account = new Account()
            {
                Username = UserName
            };
            account.Properties.Add("token", token.access_token);
            var expirationDate = DateTime.Now.AddSeconds(Double.Parse(token.expires_in));
            account.Properties.Add("expires", JsonConvert.SerializeObject(expirationDate));
            AccountStore.Create().Save(account, App.AppName);
            return true;

        }

        public static string GetToken()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            return account?.Properties["token"];
        }

        public static void DeleteToken()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, App.AppName);
            }
        }

        public static bool IsTokenValid()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            if (account == null) return false;
            var expiriationTime = JsonConvert.DeserializeObject<DateTime>(account.Properties["expires"]);
            return expiriationTime.CompareTo(DateTime.Now) > 0;
         
        }
    }
}