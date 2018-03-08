using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            const string loremipsum = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.";

            reservedList = new List<Workshop>()
            {
                new Workshop()
                {
                    Title = "Leadership in practise",
                    ShortDescription = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit",
                    Description = loremipsum,
                    Date = new DateTime(2018, 06, 17),
                    Coach = "Andrzej Nowak",
                    Place = "Windesheim"
                },
                new Workshop()
                {
                    Title = "Motivation",
                    ShortDescription = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit",
                    Description = loremipsum,
                    Date = new DateTime(2018, 06, 21),
                    Coach = "Andrzej Norek",
                    Place = "Łódź"
                }
            };
            WorkshopsListView.ItemsSource = reservedList; 
        }

 
        

        private void SearchWorkshop_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SearchWorkshop.Text != null)
            {
                var displayList = reservedList.Where(x => x.Title.Contains(SearchWorkshop.Text)).ToList();
                WorkshopsListView.ItemsSource = displayList;
            }
            else
            {
                WorkshopsListView.ItemsSource = reservedList;
            }
        }
    }
}
