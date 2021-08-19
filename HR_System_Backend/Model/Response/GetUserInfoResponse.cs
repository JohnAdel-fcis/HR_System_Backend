using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class GetUserInfoResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public int privilage { get; set; }
        public bool enabled { get; set; }
    }
}
