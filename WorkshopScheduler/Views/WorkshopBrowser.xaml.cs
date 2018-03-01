using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace WorkshopScheduler
{
    public partial class WorkshopBrowser : ContentPage
    {

        public WorkshopBrowser()
        {
            InitializeComponent();
            //lorem ipsum is to test longer strings, but I dont want to deal with them normally ;) 
            String loremipsum = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.";

            var workshops_list = new List<Workshop>();

            workshops_list.Add(new Workshop() {
               Title =  "Project Management",
                ShortDescription = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit",
                Description =  loremipsum,
                Date = new DateTime(2018,06,17),
                 Coach = "Andrzej Nowak",
                Place = "Windesheim"});

            workshopsList.ItemsSource = workshops_list; 
        }

        void onSignIn (object sender, System.EventArgs e)
        {
            DisplayAlert("Sign in", "Are you sure?","Yes","No");
        }
    }
}
