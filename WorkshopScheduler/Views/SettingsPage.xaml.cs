using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Messaging;
using WorkshopScheduler.Logic;
using WorkshopScheduler.Views.UserAccountViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkshopScheduler.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
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
                    .Subject("Test")
                    .BodyAsHtml("<h1> TEST </h1> <br> Can I go to this party... <br> <h3>MOOOOOM PLEEEAASE</h3>")
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