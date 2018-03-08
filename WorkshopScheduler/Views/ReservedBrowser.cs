using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class ReservedBrowser : ContentPage
    {

        List<Workshop> reservedList;

        public ReservedBrowser()
        {
            InitializeComponent();
            //lorem ipsum is to test longer strings, but I dont want to deal with them normally ;) 
            String loremipsum = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.";

            reservedList = new List<Workshop>();

            reservedList.Add(new Workshop() {
               Title =  "Leadership in practise",
                ShortDescription = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit",
                Description =  loremipsum,
                Date = new DateTime(2018,06,17),
                Coach = "Andrzej Nowak",
                Place = "Windesheim"});

            reservedList.Add(new Workshop()
            {
                Title = "Motivation",
                ShortDescription = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit",
                Description = loremipsum,
                Date = new DateTime(2018, 06, 21), 
                Coach = "Andrzej Norek",
                Place = "Łódź"
            });

            workshopsListView.ItemsSource = reservedList; 
        }

 
        void searchboxChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
                List<Workshop> displayList;
                toTest.Text = searchWorkshop.Text;

            if(searchWorkshop.Text != null)
            {
                displayList = reservedList.Where(x => x.Title.Contains(searchWorkshop.Text)).ToList();
                workshopsListView.ItemsSource = displayList; 
            }else
            {
                workshopsListView.ItemsSource = reservedList;
            }
           
           

        }
    }
}
