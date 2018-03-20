using System;
using Xamarin.Forms;
using WorkshopScheduler.Views;

namespace WorkshopScheduler
{
    public partial class App : Application
    {
        private const string UserNameKey = "UserName";
        private const string UserSurnameKey = "UserSurname";
        private const string UserUnitKey = "UserUnit";

        public App()
        {
            InitializeComponent();
            MainPage = new MainView();
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

        public String UserUnit
        {
            get => Properties.ContainsKey(UserUnitKey) ? Properties[UserUnitKey].ToString() : "";
            set => Properties[UserUnitKey] = value;
        }

    }
}
