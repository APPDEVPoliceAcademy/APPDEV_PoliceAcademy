using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkshopScheduler.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToolbarView : NavigationPage
    {
        public ToolbarView()
        {
            InitializeComponent();
            PushAsync(new MainView());
        }

        private void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Elo", "siema", "czesc");
        }
    }
}