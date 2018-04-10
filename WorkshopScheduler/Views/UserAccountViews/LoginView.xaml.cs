using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkshopScheduler.Logic;
using WorkshopScheduler.RestLogic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkshopScheduler.Views.UserAccountViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginView : ContentPage
	{
		public LoginView ()
		{
			InitializeComponent ();
		}

	    private void CreateAccountButton_OnClicked(object sender, EventArgs e)
	    {
	        Navigation.PushModalAsync(new RegisterView());
	    }

	    private async void LogInButton_OnClicked(object sender, EventArgs e)
	    {

	        if (Login.Text == null || Password.Text == null) return;

            IRestService rs = new RestService();

	        var tokenResponse = await rs.AuthenticateUser(Login.Text, Password.Text);

	        if (tokenResponse.ResponseCode == null)
	        {
	            await DisplayAlert("Error", tokenResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
	            return;
	        }

	        if (tokenResponse.ResponseCode == HttpStatusCode.BadRequest)
	        {
	            await DisplayAlert("Error", tokenResponse.ErrorMessage, "Ok");
	        }

	        if (tokenResponse.ResponseCode == HttpStatusCode.OK)
	        {
	            TokenManager.SaveToken(tokenResponse.Value);

	            var userInfo = await rs.GetUserDetail();
	            var app = Application.Current as App;
	            app.UserName = userInfo.Value.Name;
	            app.UserSurname = userInfo.Value.Surname;
	            app.UserUnit = userInfo.Value.Unit;

	            Application.Current.MainPage = new MainView();
            }

	        
	    }
	}
}