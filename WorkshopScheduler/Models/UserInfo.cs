using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopScheduler.Models
{
    public enum Unit
    {
        Noord,
        Soud,
        Noone
    }


    public class UserInfo
    {
        public String Name { get; set; }
        public String Surname { get; set; }
        public Unit Unit { get; set; }
    }
}
