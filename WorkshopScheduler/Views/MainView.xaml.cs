﻿using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class MainView : TabbedPage
    {
        public MainView()
        {
            InitializeComponent();

            Children.Add(new WorkshopBrowser());
            Children.Add(new ReservedBrowser());
        }
    }
}