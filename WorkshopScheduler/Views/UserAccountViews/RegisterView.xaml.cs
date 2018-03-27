using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            // Validate that user account with this login does not exist

            IRestService restService = new RestService();

	        var tokenInfo = await restService.CreateUser(Login.Text, FirstPassword.Text);
	        if (tokenInfo == null)
	        {
	            await DisplayAlert("Error", "This login was already taken", "Goed");
	            return;
	        }

	        TokenManager.SaveToken(tokenInfo.Access_token);



	        Navigation.PushModalAsync(new ProfileDetailPage());
	    }
	}
}