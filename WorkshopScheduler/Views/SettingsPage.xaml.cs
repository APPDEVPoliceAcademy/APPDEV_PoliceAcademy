using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopScheduler.Logic;
using WorkshopScheduler.Views.UserAccountViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkshopScheduler.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void LogOutButton_OnClicked(object sender, EventArgs e)
        {
            TokenManager.DeleteToken();
            Application.Current.MainPage = new LoginView();

        }
    }
}