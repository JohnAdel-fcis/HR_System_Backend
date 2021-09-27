﻿using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class Employee
    {
        public Employee()
        {
            BounseDiscounts = new HashSet<BounseDiscount>();
            Covenants = new HashSet<Covenant>();
            Debits = new HashSet<Debit>();
            Documents = new HashSet<Document>();
            ExcusedAbsences = new HashSet<ExcusedAbsence>();
            FingerLogs = new HashSet<FingerLog>();
            ItemTransactions = new HashSet<ItemTransaction>();
            Items = new HashSet<Item>();
            WorkTimes = new HashSet<WorkTime>();
        }

        public int Id { get; set; }
        public int? Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public decimal? Salary { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
        public int? AllowCome { get; set; }
        public int? AllowOut { get; set; }
        public int? BaseTime { get; set; }
        public int? DepartmentId { get; set; }
        public int? CategoryId { get; set; }
        public int? SalaryTypeId { get; set; }
        public int? ShiftId { get; set; }
        public bool? Productivity { get; set; }
        public int? RoleId { get; set; }
        public int? DeviceId { get; set; }
        public string Password { get; set; }
        public decimal? MedicalInsurance { get; set; }
        public decimal? SocialInsurance { get; set; }
        public bool? SocialInsurancePercentage { get; set; }
        public bool? MedicalInsurancePercentage { get; set; }
        public string ProfilePicPath { get; set; }
        public bool? Deleted { get; set; }
        public int? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Category Category { get; set; }
        public virtual Department Department { get; set; }
        public virtual Role Role { get; set; }
        public virtual SalaryType SalaryType { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual Holiday Holiday { get; set; }
        public virtual WorkDay WorkDay { get; set; }
        public virtual ICollection<BounseDiscount> BounseDiscounts { get; set; }
        public virtual ICollection<Covenant> Covenants { get; set; }
        public virtual ICollection<Debit> Debits { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<ExcusedAbsence> ExcusedAbsences { get; set; }
        public virtual ICollection<FingerLog> FingerLogs { get; set; }
        public virtual ICollection<ItemTransaction> ItemTransactions { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<WorkTime> WorkTimes { get; set; }
    }
}
