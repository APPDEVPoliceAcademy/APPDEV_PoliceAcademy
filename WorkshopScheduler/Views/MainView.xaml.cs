using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class MainView : TabbedPage
    {
        public MainView()
        {
            InitializeComponent();
            WorkshopBrowser.WorkshopEnrolled += ReservedBrowser.OnWorkshopEnrolled;
            ReservedBrowser.UserDisenrolled += WorkshopBrowser.OnWorkshopDisenrolled;

        }
    }
}
