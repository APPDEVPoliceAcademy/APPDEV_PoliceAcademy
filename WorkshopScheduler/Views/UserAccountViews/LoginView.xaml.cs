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

	        var tokenInfo = await rs.AuthenticateUser(Login.Text, Password.Text);
	        if (tokenInfo == null)
	        {
	            await DisplayAlert("Error", "Authentication failed", "Ok");
	            return;
	        }

	        TokenManager.SaveToken(tokenInfo.Access_token);

	        var userInfo = await rs.GetUserDetail();
	        var app = Application.Current as App;
	        app.UserName = userInfo.Name;
	        app.UserSurname = userInfo.Surname;
	        app.UserUnit = userInfo.Unit;

            Application.Current.MainPage = new MainView();
	    }
	}
}