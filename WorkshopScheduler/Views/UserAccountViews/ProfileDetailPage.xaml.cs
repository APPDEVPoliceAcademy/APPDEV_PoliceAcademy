using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
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

        private List<Unit> _pickerUnitsOptions;
        public ProfileDetailPage()
        {
            InitializeComponent();
            _pickerUnitsOptions = Enum.GetValues(typeof(Unit)).Cast<Unit>().ToList();
            UnitPicker.ItemsSource = _pickerUnitsOptions;

        }

        private async void ConfirmInformation_OnClicked(object sender, EventArgs e)
        {
            IRestService restService = new RestService();

            var userInfo = new UserInfo()
            {
                Name = NameCell.Text,
                Surname = NameCell.Text,
                Unit = (Unit)Enum.Parse(typeof(Unit), UnitPicker.Items[UnitPicker.SelectedIndex])            
             };


            var response = await restService.UpdateUserInfo(userInfo);

            if (response.ResponseCode == null)
            {
                await DisplayAlert("Error", response.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
                return;
            }

            if (response.ResponseCode == HttpStatusCode.BadRequest)
            {
                await DisplayAlert("Error", response.ErrorMessage, "Ok");
            }

            if (response.ResponseCode == HttpStatusCode.OK)
            {
                Application.Current.MainPage = new MainView();
            }

        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}