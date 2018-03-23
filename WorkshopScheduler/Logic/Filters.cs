using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using WorkshopScheduler.Models;
using System.Collections.ObjectModel;

namespace WorkshopScheduler.Logic
{

    public class Filters
    {


        public ObservableCollection<Workshop> FilterByDate(ObservableCollection<Workshop> input, DateTime[] dates)
        {
            return new ObservableCollection<Workshop>(input.Where(a => (a.Date >= dates[0].Date && a.Date <= dates[1].Date)));
        }

        public ObservableCollection<Workshop> FilterByPlace(ObservableCollection<Workshop> input, String desiredPlace)
        {
            return new ObservableCollection<Workshop>(input.Where(a => (a.Place == desiredPlace)));
        }

        public ObservableCollection<Workshop> FilterBy12weeks(ObservableCollection<Workshop> input, bool flag)
        {
            if (flag)
                return null;
            else
                return input;
        }
    }


}