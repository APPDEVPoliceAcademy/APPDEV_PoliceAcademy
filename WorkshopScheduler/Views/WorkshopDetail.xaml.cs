using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopScheduler.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkshopScheduler.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WorkshopDetail : ContentPage
	{
		public WorkshopDetail (Workshop currentWorkshop)
		{
			InitializeComponent ();
		    BindingContext = currentWorkshop;
		}

	    private void BackButton_OnClicked(object sender, EventArgs e)
	    {
	        Navigation.PopModalAsync();
	    }

	    private void ApplyButton_OnClicked(object sender, EventArgs e)
	    {
	        if ((BindingContext as Workshop).IsWithin12Weeks)
	        {
	            DisplayAlert("Alert", "You are about to sign for workshop within 12 weeks\n" +
	                                  "Do you want to send email to your scheduler?",
	                "Yes take me to mail app",
	                "Continue anyway");
	        }
	        else
	        {
	            DisplayAlert("Sign in", "Signing in for workshop", "Ok");
	        }
	    }
	}
}