using HR_System_Backend.Model.Helper;
using HR_System_Backend.Model.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class EmployeeResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public DateTime? createdDate { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public decimal? salary { get; set; }
        public string timeIn { get; set; }
        public string timeOut { get; set; }
        public int? allowCome { get; set; }
        public int? allowOut { get; set; }
        public int? baseTime { get; set; }
        public int? departmentId { get; set; }
        public int? categoryId { get; set; }
        public int? salaryId { get; set; }
        public int? shiftId { get; set; }
        public Week holiday { get; set; }
        public Week workDays { get; set; }
        public List<string> documents { get; set; }
        public int? roleId { get; set; }
        public int? deviceId { get; set; }
        public string password { get; set; }
        public bool productivity { get; set; }
        public List<ItemResponse> items { get; set; }
        public int? code { get; set; }
    }
}
