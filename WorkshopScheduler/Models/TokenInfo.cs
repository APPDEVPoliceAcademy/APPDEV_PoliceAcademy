using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopScheduler.Models
{
    public class TokenInfo
    {
        public String Access_token { get; set; }
        public String Token_type { get; set; }
        public String Expires_in { get; set; }
    }
}
