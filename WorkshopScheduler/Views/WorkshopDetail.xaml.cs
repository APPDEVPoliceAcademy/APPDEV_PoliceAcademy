using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkshopScheduler.Models;
using WorkshopScheduler.RestLogic;
using WorkshopScheduler.Views.UserAccountViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkshopScheduler.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkshopDetail : ContentPage
    {
        private IRestService _restService = new RestService();
        private int _currentWorkshopID;

        public WorkshopDetail(int currentWorkshopID)
        {
            _currentWorkshopID = currentWorkshopID;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            var workshopsResponse = await _restService.GetSingleWorkshop(_currentWorkshopID);

            if (workshopsResponse.ResponseCode == null)
            {
                await DisplayAlert("Error",
                    workshopsResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
                Navigation.PopAsync();
            }

            if (workshopsResponse.ResponseCode == HttpStatusCode.Unauthorized)
            {
                //Check token validation additionaly
                await DisplayAlert("Error", "Your session has expired. You will be redirected to log in", "Ok");
                Application.Current.MainPage = new LoginView();
            }

            if (workshopsResponse.ResponseCode == HttpStatusCode.OK)
            {
                BindingContext = workshopsResponse.Value;
            }


            base.OnAppearing();
        }


        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void ApplyButton_OnClicked(object sender, EventArgs e)
        {
            if ((BindingContext as Workshop).IsWithin12Weeks)
            {
                await DisplayAlert("Alert", "You are about to sign for workshop within 12 weeks\n" +
                                      "Do you want to send email to your scheduler?",
                    "Yes take me to mail app",
                    "Continue anyway");
            }
            else
            {
                await DisplayAlert("Sign in", "Signing in for workshop", "Ok");
            }

            var restResponse = await _restService.EnrollUser((BindingContext as Workshop).Id);

            if (restResponse.ResponseCode == null)
            {
                await DisplayAlert("Error",
                    restResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
                return;
            }

            if (restResponse.ResponseCode == HttpStatusCode.Unauthorized)
            {
                //Check token validation additionaly
                await DisplayAlert("Error", "Your session has expired. You will be redirected to log in", "Ok");
                Application.Current.MainPage = new LoginView();
            }

            if (restResponse.ResponseCode == HttpStatusCode.OK)
            {
                await Navigation.PopModalAsync();
            }
        }
    }
}