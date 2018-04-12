using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class MainView : TabbedPage
    {
        public MainView()
        {
            InitializeComponent();
            WorkshopBrowser.UserEnrolled += ReservedBrowser.OnUserEnrolled;
            WorkshopBrowser.UserDisenrolled += ReservedBrowser.OnUserDisenrolled;
            ReservedBrowser.UserDisenrolled += WorkshopBrowser.OnWorkshopDisenrolled;

        }
    }
}
