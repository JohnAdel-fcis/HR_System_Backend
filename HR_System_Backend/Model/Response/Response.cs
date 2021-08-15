using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class Response<T>
    {
        public Response()
        {
            data = new List<T>();
        }
        public bool     status  { get; set; }
        public string   message { get; set; }
        public List<T>  data    { get; set; }
    }
}
