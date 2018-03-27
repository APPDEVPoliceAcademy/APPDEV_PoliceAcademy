﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace WorkshopScheduler.Logic
{
    public static class TokenManager
    {

        private const string _userName = "Local";

        public static bool SaveToken(string token)
        {
            if (String.IsNullOrEmpty(token)) return false;
            var account = new Account()
            {
                Username = _userName
            };
            account.Properties.Add("token", token);
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
    }
}