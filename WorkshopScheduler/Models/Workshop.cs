using System;
namespace WorkshopScheduler.Models
{
    public class Workshop
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime Date { get; set; }
        public string Coach { get; set; }
        public string Place { get; set; }

        public bool IsWithin12Weeks
        {
            //
            get { return Date.CompareTo(DateTime.Now.AddDays(12 * 7)) < 0; }
        }


        public override bool Equals(object obj)
        {
            Workshop workshopObj = obj as Workshop; 
            if (workshopObj == null)
                return false;
            else
            {
            
                bool areSame = this.Title.Equals(workshopObj.Title) &&
                               this.Date.Equals(workshopObj.Date) &&
                               this.Coach.Equals(workshopObj.Coach) &&
                               this.Description.Equals(workshopObj.Description) &&
                               this.Place.Equals(workshopObj.Place) &&
                               this.ShortDescription.Equals(workshopObj.ShortDescription);
                return areSame;
            }
        }
    }

}
