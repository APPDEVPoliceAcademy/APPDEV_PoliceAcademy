using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopScheduler.RestLogic
{
    public class RestResponse<T>
    {
        public T Value { get; set; }
        public HttpStatusCode? ResponseCode { get; set; }
        public string ErrorMessage { get; set; }    
        
        public RestResponse()
        {
            ResponseCode = null;
            ErrorMessage = null;
        }
    }
}
