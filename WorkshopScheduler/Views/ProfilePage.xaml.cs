using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Plugin.Messaging;
using WorkshopScheduler.Logic;
using WorkshopScheduler.Models;
using WorkshopScheduler.RestLogic;
using WorkshopScheduler.Views.UserAccountViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace WorkshopScheduler.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {

        private App _currentApp = Application.Current as App;
        private readonly List<Unit> _pickerUnitsOptions = Enum.GetValues(typeof(Unit)).Cast<Unit>().ToList();
        private IRestService _restService;

        public ProfilePage()
        {
            InitializeComponent();
            _restService = new RestService();
            NameCell.Text = _currentApp.UserName;
            SurnameCell.Text = _currentApp.UserSurname;
            UnitPicker.ItemsSource = _pickerUnitsOptions;
            UnitPicker.SelectedItem = _currentApp.UserUnit;
            BirthdayCell.Date = _currentApp.UserBirthday;
        }

        async void DoneButton_OnClicked(object sender, System.EventArgs e)
        {
            if (NameCell.Text == _currentApp.UserName &&
                SurnameCell.Text == _currentApp.UserSurname &&
                UnitPicker.ItemsSource == _pickerUnitsOptions &&
                (Unit)Enum.Parse(typeof(Unit), UnitPicker.Items[UnitPicker.SelectedIndex]) == _currentApp.UserUnit &&
                BirthdayCell.Date == _currentApp.UserBirthday)
            {
                return;
            }
             var userInfo = new UserInfo()
            {
                Birthday = BirthdayCell.Date,
                Name = NameCell.Text,
                Surname = SurnameCell.Text,
                Unit = (Unit) Enum.Parse(typeof(Unit), UnitPicker.Items[UnitPicker.SelectedIndex])
            };

            var response  = await _restService.UpdateUserInfo(userInfo);

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
            }


        }

        private void LogOutButton_OnClicked(object sender, EventArgs e)
        {
            TokenManager.DeleteToken();
            Application.Current.MainPage = new LoginView();

        }

        private void IdeaButton_OnClicked(object sender, EventArgs e)
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;

            if (emailMessenger.CanSendEmail)
            {
                var email = new EmailMessageBuilder()
                    .To("wyrzuc.maciej@gmail.com")
                    .Cc("krymarys.jakub@gmail.com")
                    .Subject("Ik heb een Idee")
                    .BodyAsHtml("Below please discribe concisely the vision of workshop you have")
                    .Build();
                Task.Delay(100);
                emailMessenger.SendEmail(email);
            }
            else
            {
                DisplayAlert("Whoops!", "This device cannot send the mails", "ok");
            }
        }

       
    }
}