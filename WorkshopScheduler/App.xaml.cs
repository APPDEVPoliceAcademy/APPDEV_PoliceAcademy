using System;
using System.IO;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Plugin.LocalNotifications;
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
        private const string UserEvaluatedKey = "UserEvaluated";
        private const string UserNextNotification = "UserNotifcation";

        public static string AppName => "WorkshopApp";

        private bool _isNotificationPlanned;


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

            if (_isNotificationPlanned && UserNotifcationTime < DateTime.Now)
            {
                _isNotificationPlanned = false;
            }
        }

        protected override void OnSleep()
        {
            if (!_isNotificationPlanned && UserEvaluated > 0)
            {

                DateTime nextNotificationDate;
                
                if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 16)
                {
                    nextNotificationDate = DateTime.Now.AddHours(5);
                }
                else if(DateTime.Now.Hour < 7)
                {
                    nextNotificationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0);
                }
                else
                {
                    nextNotificationDate = DateTime.Now.AddHours(16);
                }
                
                UserNotifcationTime = nextNotificationDate;

                CrossLocalNotifications.Current.Show("Remainder", String.Format("You have {0} not evaluated workshops", UserEvaluated), 101,  nextNotificationDate);
                _isNotificationPlanned = true;
            }

        }

        protected override void OnResume()
        {
            if (_isNotificationPlanned && UserNotifcationTime < DateTime.Now)
            {
                _isNotificationPlanned = false;
            }
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
            get => Properties.ContainsKey(UserUnitKey) ? (Unit) Enum.Parse(typeof(Unit), Properties[UserUnitKey].ToString()) : Unit.Noord;
            set => Properties[UserUnitKey] = value.ToString();
        }

        public DateTime UserBirthday
        {
            get => Properties.ContainsKey(UserBirthdayKey) ? (DateTime)Properties[UserBirthdayKey] : DateTime.Now;
            set => Properties[UserBirthdayKey] = value;

        }

        public DateTime UserNotifcationTime
        {
            get => Properties.ContainsKey(UserNextNotification) ? (DateTime)Properties[UserNextNotification] : DateTime.Now.Subtract(TimeSpan.FromSeconds(10));
            set => Properties[UserNextNotification] = value;

        }

        public int UserEvaluated
        {
            get => Properties.ContainsKey(UserEvaluatedKey) ? (int) Properties[UserEvaluatedKey] : 0;
            set => Properties[UserEvaluatedKey] = value;
        }
    }
}
