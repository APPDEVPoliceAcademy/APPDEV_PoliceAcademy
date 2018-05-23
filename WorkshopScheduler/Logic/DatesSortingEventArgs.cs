using System;
namespace WorkshopScheduler.Logic
{
    public class DatesSortingEventArgs : EventArgs
    {

        public DateTime[] dates;
        public bool isOn;
    }
}
