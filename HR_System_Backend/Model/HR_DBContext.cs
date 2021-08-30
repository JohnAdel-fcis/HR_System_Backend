using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class HR_DBContext : DbContext
    {
        public HR_DBContext()
        {
        }

        public HR_DBContext(DbContextOptions<HR_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BounseDiscount> BounseDiscounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Covenant> Covenants { get; set; }
        public virtual DbSet<Debit> Debits { get; set; }
        public virtual DbSet<DebitTransaction> DebitTransactions { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<FingerLog> FingerLogs { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemTransaction> ItemTransactions { get; set; }
        public virtual DbSet<OverTime> OverTimes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SalaryType> SalaryTypes { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<WorkDay> WorkDays { get; set; }
        public virtual DbSet<WorkTime> WorkTimes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=PHS-IT53\\;Database=HR_DB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BounseDiscount>(entity =>
            {
                entity.HasKey(e => e.BdId)
                    .HasName("PK__BOUNSE_D__DACD68A92ECA3EA7");

                entity.ToTable("BOUNSE_DISCOUNT");

                entity.Property(e => e.BdId).HasColumnName("BD_ID");

                entity.Property(e => e.AddedBy)
                    .HasMaxLength(50)
                    .HasColumnName("ADDED_BY");

                entity.Property(e => e.Amount).HasColumnName("AMOUNT");

                entity.Property(e => e.Bonuse)
                    .HasColumnName("BONUSE")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("DATE");

                entity.Property(e => e.Discount)
                    .HasColumnName("DISCOUNT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EmployeeId).HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("NAME");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.BounseDiscounts)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__BOUNSE_DI__EMPLO__5CD6CB2B");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("CATEGORY");

                entity.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .HasColumnName("CATEGORY_NAME");
            });

            modelBuilder.Entity<Covenant>(entity =>
            {
                entity.ToTable("COVENANT");

                entity.Property(e => e.CovenantId).HasColumnName("COVENANT_ID");

                entity.Property(e => e.CovenantFromDate)
                    .HasColumnType("date")
                    .HasColumnName("COVENANT_FROM_DATE");

                entity.Property(e => e.CovenantName)
                    .HasMaxLength(200)
                    .HasColumnName("COVENANT_NAME");

                entity.Property(e => e.CovenantPath)
                    .HasMaxLength(200)
                    .HasColumnName("COVENANT_PATH");

                entity.Property(e => e.CovenantToDate)
                    .HasColumnType("date")
                    .HasColumnName("COVENANT_TO_DATE");

                entity.Property(e => e.EmplyeeId).HasColumnName("EMPLYEE_ID");

                entity.HasOne(d => d.Emplyee)
                    .WithMany(p => p.Covenants)
                    .HasForeignKey(d => d.EmplyeeId)
                    .HasConstraintName("FK__COVENANT__EMPLYE__3A81B327");
            });

            modelBuilder.Entity<Debit>(entity =>
            {
                entity.ToTable("DEBIT");

                entity.Property(e => e.DebitId).HasColumnName("DEBIT_ID");

                entity.Property(e => e.DebitAmount).HasColumnName("DEBIT_AMOUNT");

                entity.Property(e => e.DebitDate)
                    .HasColumnType("date")
                    .HasColumnName("DEBIT_DATE");

                entity.Property(e => e.DebitName)
                    .HasMaxLength(100)
                    .HasColumnName("DEBIT_NAME");

                entity.Property(e => e.EmployeeId).HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.Finished).HasColumnName("FINISHED");

                entity.Property(e => e.Installment)
                    .HasColumnName("INSTALLMENT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InstallmentPaidAmount)
                    .HasColumnName("INSTALLMENT_PAID_AMOUNT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastInstallmentPayDate)
                    .HasColumnType("date")
                    .HasColumnName("LAST_INSTALLMENT_PAY_DATE");

                entity.Property(e => e.Notes)
                    .HasMaxLength(200)
                    .HasColumnName("NOTES");

                entity.Property(e => e.PaidAmount)
                    .HasColumnName("PAID_AMOUNT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RemainingDebitAmount)
                    .HasColumnName("REMAINING_DEBIT_AMOUNT")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Debits)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__DEBIT__EMPLOYEE___489AC854");
            });

            modelBuilder.Entity<DebitTransaction>(entity =>
            {
                entity.HasKey(e => e.TranId)
                    .HasName("PK__DEBIT_TR__C314C336E6AAA33D");

                entity.ToTable("DEBIT_TRANSACTIONS");

                entity.Property(e => e.TranId).HasColumnName("TRAN_ID");

                entity.Property(e => e.DebitId).HasColumnName("DEBIT_ID");

                entity.Property(e => e.InstallmentPaidAmount)
                    .HasColumnName("INSTALLMENT_PAID_AMOUNT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Instalment)
                    .HasColumnName("INSTALMENT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsInstalment).HasColumnName("IS_INSTALMENT");

                entity.Property(e => e.LastInstallmentPayDate)
                    .HasColumnType("datetime")
                    .HasColumnName("LAST_INSTALLMENT_PAY_DATE");

                entity.Property(e => e.Notes)
                    .HasMaxLength(200)
                    .HasColumnName("NOTES");

                entity.Property(e => e.PaidAmount)
                    .HasColumnName("PAID_AMOUNT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PartialPayment)
                    .HasColumnName("PARTIAL_PAYMENT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RemainingDebitAmount)
                    .HasColumnName("REMAINING_DEBIT_AMOUNT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TranDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TRAN_DATE");

                entity.HasOne(d => d.Debit)
                    .WithMany(p => p.DebitTransactions)
                    .HasForeignKey(d => d.DebitId)
                    .HasConstraintName("FK__DEBIT_TRA__DEBIT__5E8A0973");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("DEPARTMENT");

                entity.Property(e => e.DepartmentId).HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(50)
                    .HasColumnName("DEPARTMENT_NAME");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("DEVICES");

                entity.Property(e => e.DeviceId).HasColumnName("DEVICE_ID");

                entity.Property(e => e.DeviceIp)
                    .HasMaxLength(30)
                    .HasColumnName("DEVICE_IP");

                entity.Property(e => e.DeviceName)
                    .HasMaxLength(200)
                    .HasColumnName("DEVICE_NAME")
                    .HasDefaultValueSql("('NAME')");

                entity.Property(e => e.DevicePort)
                    .HasMaxLength(10)
                    .HasColumnName("DEVICE_PORT");

                entity.Property(e => e.Priority)
                    .HasColumnName("PRIORITY")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("DOCUMENT");

                entity.Property(e => e.DocumentId).HasColumnName("DOCUMENT_ID");

                entity.Property(e => e.AddedBy)
                    .HasMaxLength(100)
                    .HasColumnName("ADDED_BY");

                entity.Property(e => e.DocumentName)
                    .HasMaxLength(100)
                    .HasColumnName("DOCUMENT_NAME");

                entity.Property(e => e.DocumentPath)
                    .HasMaxLength(200)
                    .HasColumnName("DOCUMENT_PATH");

                entity.Property(e => e.EmployeeId).HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.UploadDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPLOAD_DATE");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__DOCUMENT__EMPLOY__3D5E1FD2");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("EMPLOYEE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.AllowCome).HasColumnName("ALLOW_COME");

                entity.Property(e => e.AllowOut).HasColumnName("ALLOW_OUT");

                entity.Property(e => e.BaseTime).HasColumnName("BASE_TIME");

                entity.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID");

                entity.Property(e => e.Code).HasColumnName("CODE");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("CREATE_DATE");

                entity.Property(e => e.DepartmentId).HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.DeviceId).HasColumnName("DEVICE_ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.MedicalInsurance)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("MEDICAL_INSURANCE");

                entity.Property(e => e.MedicalInsurancePercentage).HasColumnName("MEDICAL_INSURANCE_PERCENTAGE");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(20)
                    .HasColumnName("MOBILE");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("NAME");

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("PHONE");

                entity.Property(e => e.Productivity)
                    .HasColumnName("PRODUCTIVITY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.Salary)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("SALARY");

                entity.Property(e => e.SalaryTypeId).HasColumnName("SALARY_TYPE_ID");

                entity.Property(e => e.ShiftId).HasColumnName("SHIFT_ID");

                entity.Property(e => e.SocialInsurance)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("SOCIAL_INSURANCE");

                entity.Property(e => e.SocialInsurancePercentage).HasColumnName("SOCIAL_INSURANCE_PERCENTAGE");

                entity.Property(e => e.TimeIn).HasColumnName("TIME_IN");

                entity.Property(e => e.TimeOut).HasColumnName("TIME_OUT");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__EMPLOYEE__CATEGO__267ABA7A");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__EMPLOYEE__DEPART__25869641");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK__EMPLOYEE__DEVICE__2CBDA3B5");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__EMPLOYEE__ROLE_I__2BC97F7C");

                entity.HasOne(d => d.SalaryType)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.SalaryTypeId)
                    .HasConstraintName("FK__EMPLOYEE__SALARY__276EDEB3");

                entity.HasOne(d => d.Shift)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.ShiftId)
                    .HasConstraintName("FK__EMPLOYEE__SHIFT___4BAC3F29");
            });

            modelBuilder.Entity<FingerLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__FINGER_L__4364C882284553C2");

                entity.ToTable("FINGER_LOGS");

                entity.Property(e => e.LogId).HasColumnName("LOG_ID");

                entity.Property(e => e.Code).HasColumnName("CODE");

                entity.Property(e => e.EmpId).HasColumnName("EMP_ID");

                entity.Property(e => e.InOut).HasColumnName("IN_OUT");

                entity.Property(e => e.LogDate)
                    .HasColumnType("date")
                    .HasColumnName("LOG_DATE");

                entity.Property(e => e.LogTime).HasColumnName("LOG_TIME");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.FingerLogs)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK__FINGER_LO__EMP_I__756D6ECB");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.HasKey(e => e.HolidaysId)
                    .HasName("PK__HOLIDAYS__1B6383DEC538149F");

                entity.ToTable("HOLIDAYS");

                entity.HasIndex(e => e.EmployeeId, "EmpHoliday")
                    .IsUnique();

                entity.Property(e => e.HolidaysId).HasColumnName("HOLIDAYS_ID");

                entity.Property(e => e.EmployeeId).HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.Friday)
                    .HasColumnName("FRIDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Monday)
                    .HasColumnName("MONDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Saturday)
                    .HasColumnName("SATURDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Sunday)
                    .HasColumnName("SUNDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Thursday)
                    .HasColumnName("THURSDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Tuesday)
                    .HasColumnName("TUESDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Wednesday)
                    .HasColumnName("WEDNESDAY")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Employee)
                    .WithOne(p => p.Holiday)
                    .HasForeignKey<Holiday>(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__HOLIDAYS__EMPLOY__3B40CD36");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("ITEMS");

                entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");

                entity.Property(e => e.EmpId).HasColumnName("EMP_ID");

                entity.Property(e => e.ItemCommission).HasColumnName("ITEM_COMMISSION");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(100)
                    .HasColumnName("ITEM_NAME");

                entity.Property(e => e.ItemPrice).HasColumnName("ITEM_PRICE");

                entity.Property(e => e.ItemQnty).HasColumnName("ITEM_QNTY");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ITEMS__EMP_ID__0880433F");
            });

            modelBuilder.Entity<ItemTransaction>(entity =>
            {
                entity.HasKey(e => e.TarnsId)
                    .HasName("PK__ITEM_TRA__A513AD9D16CA8404");

                entity.ToTable("ITEM_TRANSACTIONS");

                entity.Property(e => e.TarnsId).HasColumnName("TARNS_ID");

                entity.Property(e => e.EmpId).HasColumnName("EMP_ID");

                entity.Property(e => e.ItemComissions).HasColumnName("ITEM_COMISSIONS");

                entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");

                entity.Property(e => e.ItemQuantity).HasColumnName("ITEM_QUANTITY");

                entity.Property(e => e.TransDate)
                    .HasColumnType("date")
                    .HasColumnName("TRANS_DATE");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.ItemTransactions)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ITEM_TRAN__EMP_I__27F8EE98");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemTransactions)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK__ITEM_TRAN__ITEM___2704CA5F");
            });

            modelBuilder.Entity<OverTime>(entity =>
            {
                entity.ToTable("OVER_TIME");

                entity.Property(e => e.OverTimeId).HasColumnName("OVER_TIME_ID");

                entity.Property(e => e.Notes)
                    .HasMaxLength(200)
                    .HasColumnName("NOTES");

                entity.Property(e => e.OverHourPrice).HasColumnName("OVER_HOUR_PRICE");

                entity.Property(e => e.OverTimeDate)
                    .HasColumnType("date")
                    .HasColumnName("OVER_TIME_DATE");

                entity.Property(e => e.OverTimeHours).HasColumnName("OVER_TIME_HOURS");

                entity.Property(e => e.OverTimePercentage).HasColumnName("OVER_TIME_PERCENTAGE");

                entity.Property(e => e.OverTimeTotal).HasColumnName("OVER_TIME_TOTAL");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ROLE");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("ROLE_NAME");
            });

            modelBuilder.Entity<SalaryType>(entity =>
            {
                entity.ToTable("SALARY_TYPE");

                entity.Property(e => e.SalaryTypeId).HasColumnName("SALARY_TYPE_ID");

                entity.Property(e => e.SalaryTypeName)
                    .HasMaxLength(50)
                    .HasColumnName("SALARY_TYPE_NAME");
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("SHIFT");

                entity.Property(e => e.ShiftId).HasColumnName("SHIFT_ID");

                entity.Property(e => e.AllowCome)
                    .HasColumnName("ALLOW_COME")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AllowLeave)
                    .HasColumnName("ALLOW_LEAVE")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DateFrom)
                    .HasColumnType("date")
                    .HasColumnName("DATE_FROM");

                entity.Property(e => e.DateTo)
                    .HasColumnType("date")
                    .HasColumnName("DATE_TO");

                entity.Property(e => e.ShiftHour)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("SHIFT_HOUR");

                entity.Property(e => e.ShiftName)
                    .HasMaxLength(100)
                    .HasColumnName("SHIFT_NAME");

                entity.Property(e => e.TimeFrom).HasColumnName("TIME_FROM");

                entity.Property(e => e.TimeTo).HasColumnName("TIME_TO");
            });

            modelBuilder.Entity<WorkDay>(entity =>
            {
                entity.HasKey(e => e.WorkDaysId)
                    .HasName("PK__WORK_DAY__4084313A6B941B3C");

                entity.ToTable("WORK_DAYS");

                entity.HasIndex(e => e.EmployeeId, "EmpWork")
                    .IsUnique();

                entity.Property(e => e.WorkDaysId).HasColumnName("WORK_DAYS_ID");

                entity.Property(e => e.EmployeeId).HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.Friday)
                    .HasColumnName("FRIDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Monday)
                    .HasColumnName("MONDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Saturday)
                    .HasColumnName("SATURDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Sunday)
                    .HasColumnName("SUNDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Thursday)
                    .HasColumnName("THURSDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Tuesday)
                    .HasColumnName("TUESDAY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Wednesday)
                    .HasColumnName("WEDNESDAY")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Employee)
                    .WithOne(p => p.WorkDay)
                    .HasForeignKey<WorkDay>(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__WORK_DAYS__EMPLO__45BE5BA9");
            });

            modelBuilder.Entity<WorkTime>(entity =>
            {
                entity.HasKey(e => e.WorkId)
                    .HasName("PK__WORK_TIM__894A0757846A0199");

                entity.ToTable("WORK_TIME");

                entity.Property(e => e.WorkId).HasColumnName("WORK_ID");

                entity.Property(e => e.EmployeeId).HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.OverTimeId).HasColumnName("OVER_TIME_ID");

                entity.Property(e => e.WorkDate)
                    .HasColumnType("date")
                    .HasColumnName("WORK_DATE");

                entity.Property(e => e.WorkEnd).HasColumnName("WORK_END");

                entity.Property(e => e.WorkStart).HasColumnName("WORK_START");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.WorkTimes)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__WORK_TIME__EMPLO__5070F446");

                entity.HasOne(d => d.OverTime)
                    .WithMany(p => p.WorkTimes)
                    .HasForeignKey(d => d.OverTimeId)
                    .HasConstraintName("FK__WORK_TIME__OVER___5165187F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
