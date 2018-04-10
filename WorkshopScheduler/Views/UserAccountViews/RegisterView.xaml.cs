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
	            return;
	        }

	        if (Login.Text.Length < 5)
	        {
	            await DisplayAlert("Error", "Login must be at least 5 signs", "Ok");
	            return;
	        }

            //Check if login is only letters and numbers
            if (!Regex.IsMatch(Login.Text, @"^[\p{L}\p{N}]+$"))
            {
                await DisplayAlert("Error", "Login can contain only letters and numbers", "Ok");
	            return;
	        }

	        if (FirstPassword.Text != RepeatPassword.Text)
	        {
	            await DisplayAlert("Error", "Two passwords does not match", "Ok");
	            return;
	        }

	        if (FirstPassword.Text.Length < 6)
	        {
	            await DisplayAlert("Error", "Password has to have at least 6 signs", "Ok");
	            return;
	        }


            IRestService restService = new RestService();

	        var restResponse = await restService.CreateUser(Login.Text, FirstPassword.Text);

	        if (restResponse.ResponseCode == null)
	        {
	            await DisplayAlert("Error", restResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
	            return;
	        }

	        if (restResponse.ResponseCode == HttpStatusCode.BadRequest)
	        {
	            await DisplayAlert("Error", restResponse.ErrorMessage, "Ok");
	        }

	        if (restResponse.ResponseCode == HttpStatusCode.OK)
	        {
	            TokenManager.SaveToken(restResponse.Value);
	            await Navigation.PushModalAsync(new ProfileDetailPage());
            }

	    }
	}
}