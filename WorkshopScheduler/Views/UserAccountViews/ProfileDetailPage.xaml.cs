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

        IRestService _restService = new RestService();
        private readonly List<Unit> _pickerUnitsOptions = Enum.GetValues(typeof(Unit)).Cast<Unit>().ToList();
        private App _currentApp = Application.Current as App;

        public ProfileDetailPage()
        {
            InitializeComponent();
            UnitPicker.ItemsSource = _pickerUnitsOptions;

        }

        private async void ConfirmInformation_OnClicked(object sender, EventArgs e)
        {

            if (NameCell.Text == null || SurnameCell.Text == null || SurnameCell.Text == null || UnitPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Please fill all fields", "Ok");
                return;
            }

            var userInfo = new UserInfo()
            {
                Birthday = BirthdayCell.Date,
                Name = NameCell.Text,
                Surname = SurnameCell.Text,
                Unit = (Unit)Enum.Parse(typeof(Unit), UnitPicker.Items[UnitPicker.SelectedIndex])
            };

            var response = await _restService.UpdateUserInfo(userInfo);

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
                _currentApp.UserName = userInfo.Name;
                _currentApp.UserSurname = userInfo.Surname;
                _currentApp.UserUnit = userInfo.Unit;
                _currentApp.UserBirthday = userInfo.Birthday;
                await _currentApp.SavePropertiesAsync();
                await DisplayAlert("Done", "Succesfully saved new information", "Ok");
                Application.Current.MainPage = new MainView();

            }

        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}