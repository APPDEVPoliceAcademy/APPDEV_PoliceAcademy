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
            WorkshopBrowser.WorkshopEvaluated += ReservedBrowser.OnWorkshopEvaluated;

            ReservedBrowser.UserDisenrolled += WorkshopBrowser.OnUserDisenrolled;
            ReservedBrowser.WorkshopEvaluated += WorkshopBrowser.OnWorkshopEvaluated;
        }
    }
}
