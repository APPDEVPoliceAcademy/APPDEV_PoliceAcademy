using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopScheduler.Models
{
    public class WorkshopDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Coach { get; set; }
        public string Place { get; set; }
        public DateTime Date { get; set; }

        private bool _isEnrolled;
        public bool IsEnrolled
        {
            get => _isEnrolled;
            set { _isEnrolled = value;
                OnPropertyChanged("IsEnrolled");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
