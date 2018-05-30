using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WorkshopScheduler.Logic;
using WorkshopScheduler.RestLogic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkshopScheduler.Views.UserAccountViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterView : ContentPage
	{


		public RegisterView ()
		{
			InitializeComponent ();
		}

	    private async void RegisterButton_OnClicked(object sender, EventArgs e)
	    {


	        if (FirstPassword.Text == null || RepeatPassword.Text == null || Login.Text == null)
	        {
                await DisplayAlert("Error", "Vul alle velden in A.u.b.", "Ok");
	            return;
	        }

	        if (Login.Text.Length < 5)
	        {
	            await DisplayAlert("Error", "Uw login moet minimaal 5 karakters zijn", "Ok");
	            return;
	        }

            //Check if login is only letters and numbers
            if (!Regex.IsMatch(Login.Text, @"^[\p{L}\p{N}]+$"))
            {
                await DisplayAlert("Error", "Uw login mag alleen bestaan uit letters en nummers", "Ok");
	            return;
	        }

	        if (FirstPassword.Text != RepeatPassword.Text)
	        {
	            await DisplayAlert("Error", "De wachtwoorden komen niet overeen", "Ok");
	            return;
	        }

	        if (FirstPassword.Text.Length < 6)
	        {
	            await DisplayAlert("Error", "Uw wachtwoord moet minimaal 6 karakters hebben", "Ok");
	            return;
	        }

	        ActivityIndicator.IsRunning = true;
	        ActivityIndicator.IsVisible = true;


            IRestService restService = new RestService();

	        var restResponse = await restService.CreateUser(Login.Text, FirstPassword.Text);

	        if (restResponse.ResponseCode == null)
	        {
	            ActivityIndicator.IsRunning = false;
	            ActivityIndicator.IsVisible = false;
                await DisplayAlert("Error", restResponse.ErrorMessage + "\nControleer of u verbining heeft met het internet", "Ok");
	            return;
	        }

	        if (restResponse.ResponseCode == HttpStatusCode.BadRequest)
	        {
	            ActivityIndicator.IsRunning = false;
	            ActivityIndicator.IsVisible = false;
                await DisplayAlert("Error", restResponse.ErrorMessage, "Ok");
	        }

	        if (restResponse.ResponseCode == HttpStatusCode.OK)
	        {
	            
                TokenManager.SaveToken(restResponse.Value);
	            var app = Application.Current as App;
	            app.UserBirthday = DateTime.Now;
	            app.UserName = "";
	            app.UserSurname = "";
	            app.UserUnit = Unit.Noord;
	            ActivityIndicator.IsRunning = false;
	            ActivityIndicator.IsVisible = false;
                await Navigation.PushModalAsync(new ProfileDetailPage());
            }

	    }
	}
}