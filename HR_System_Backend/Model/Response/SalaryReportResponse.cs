using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class SalaryReportResponse
    {

        public int? code { get; set; }
        public string SalaryType { get; set; }
        public double? workHours { get; set; }
        public double? workDays { get; set; }
        public double? absenceDays { get; set; }
        public double? holidays { get; set; }
        public double? scheduledDays { get; set; }
        public double? dayPrice { get; set; }
        public double? itemsNumber { get; set; }
        public double? productivitySalary { get; set; }
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        public double? overTime { get; set; }
        public double? late { get; set; }
        public double? bonus { get; set; }
        public double? discount { get; set; }
        public double? installment { get; set; }
        public double?  salaryForDate { get; set; }
        public double? baseSalary { get; set; }
    }
}
