using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace WorkshopScheduler
{
    public partial class WorkshopBrowser : ContentPage
    {

        List<Workshop> workshopsList;

        public WorkshopBrowser()
        {
            InitializeComponent();
            //lorem ipsum is to test longer strings, but I dont want to deal with them normally ;) 
            String loremipsum = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.";

            workshopsList = new List<Workshop>();

            workshopsList.Add(new Workshop() {
               Title =  "Project Management",
                ShortDescription = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit",
                Description =  loremipsum,
                Date = new DateTime(2018,06,17),
                Coach = "Andrzej Nowak",
                Place = "Windesheim"});

            workshopsList.Add(new Workshop()
            {
                Title = "Test 2",
                ShortDescription = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit",
                Description = loremipsum,
                Date = new DateTime(2018, 06, 19),
                Coach = "Tadeusz Sznuk",
                Place = "on-line"
            });

            workshopsList.Add(new Workshop()
            {
                Title = "Halina,Zygfryd Harry i dziewczyna bez szpary",
                ShortDescription = "Wyszukajna youtube.... ale na własne ryzyko Ptysiu ;* \n @Melvin sorry, in Enlish this not funny :<" +
                    "" +
                    "",
                Description = loremipsum,
                Date = new DateTime(2018, 06, 21), 
                Coach = "Andrzej Norek",
                Place = "Łódź"
            });

            workshopsListView.ItemsSource = workshopsList; 
        }

        void onSignIn (object sender, System.EventArgs e)
        {
            DisplayAlert("Sign in", "Are you sure?","Yes","No");
        }

        void searchboxChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
                List<Workshop> displayList;
                toTest.Text = searchWorkshop.Text;

            if(searchWorkshop.Text != null)
            {
                displayList = workshopsList.Where(x => x.Title.Contains(searchWorkshop.Text)).ToList();
                workshopsListView.ItemsSource = displayList; 
            }else
            {
                workshopsListView.ItemsSource = workshopsList;
            }
           
           

        }
    }
}
