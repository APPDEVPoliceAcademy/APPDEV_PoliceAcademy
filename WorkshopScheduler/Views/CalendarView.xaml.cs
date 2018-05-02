using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class CalendarView : ContentPage
    {
        public CalendarView()
        {
            InitializeComponent();


            var testLabel = new Label { Text = "1" };

            for (int x = 0; x < 7; x++)
                for (int y = 0; y < 7; y++)
                    grid.Children.Add(testLabel, x, y);
        }
    }
}
