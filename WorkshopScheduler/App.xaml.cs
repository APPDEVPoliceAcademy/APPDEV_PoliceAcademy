using System;
using System.IO;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using WorkshopScheduler.Logic;
using WorkshopScheduler.Models;
using WorkshopScheduler.RestLogic;
using Xamarin.Forms;
using WorkshopScheduler.Views;
using WorkshopScheduler.Views.UserAccountViews;
using Xamarin.Forms.PlatformConfiguration;

namespace WorkshopScheduler
{
    public partial class App : Application
    {
        private const string UserNameKey = "UserName";
        private const string UserSurnameKey = "UserSurname";
        private const string UserUnitKey = "UserUnit";
        private const string UserBirthdayKey = "UserBirthday";

        public static string AppName => "WorkshopApp";



        public App()
        {
            InitializeComponent();


           // MainPage = new DayModalView();
            
			if (TokenManager.IsTokenValid())
            {
                MainPage = new MainView();
            }
            else
            {
                MainPage = new LoginView();
            }
            //MainPage = new MainView();

        }

        protected override void OnStart()
        {
            
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public String UserName
        {
            get => Properties.ContainsKey(UserNameKey) ? Properties[UserNameKey].ToString() : "";
            set => Properties[UserNameKey] = value;
        }

        public String UserSurname
        {
            get => Properties.ContainsKey(UserSurnameKey) ? Properties[UserSurnameKey].ToString() : "";
            set => Properties[UserSurnameKey] = value;
        }

        public Unit UserUnit
        {
            get => Properties.ContainsKey(UserUnitKey) ? (Unit) Enum.Parse(typeof(Unit), Properties[UserUnitKey].ToString()) : Unit.Nord;
            set => Properties[UserUnitKey] = value.ToString();
        }

        public DateTime UserBirthday
        {
            get => Properties.ContainsKey(UserBirthdayKey) ? (DateTime)Properties[UserBirthdayKey] : DateTime.Now;
            set => Properties[UserBirthdayKey] = value;

        }

    }
}
