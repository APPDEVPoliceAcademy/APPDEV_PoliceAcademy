using System;
namespace WorkshopScheduler
{
    public class Workshop
    {
        private String title;
        private String description;
        private DateTime date;
        private String coach;
        private String place;

        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Coach { get => coach; set => coach = value; }
        public string Place { get => place; set => place = value; }
    }
}
