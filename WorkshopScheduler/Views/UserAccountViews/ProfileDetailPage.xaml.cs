using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopScheduler.Models;
using WorkshopScheduler.RestLogic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkshopScheduler.Views.UserAccountViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileDetailPage : ContentPage
    {
        public ProfileDetailPage()
        {
            InitializeComponent();
        }

        private void ConfirmInformation_OnClicked(object sender, EventArgs e)
        {
            IRestService restService = new RestService();

            var userInfo = new UserInfo()
            {
                Name = NameCell.Text,
                Surname = NameCell.Text,
                Unit = UnitPicker.Items.ElementAt(UnitPicker.SelectedIndex)
            };


            restService.UpdateUserInfo(userInfo);

            Application.Current.MainPage = new MainView();

        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}