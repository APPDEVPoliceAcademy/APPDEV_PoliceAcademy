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
    }
}