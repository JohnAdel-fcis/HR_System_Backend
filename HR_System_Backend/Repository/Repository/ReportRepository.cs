using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using System.Linq;
using HR_System_Backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace HR_System_Backend.Repository.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly HR_DBContext _context;
        public ReportRepository(HR_DBContext context)
        {
            _context = context;
        }
        public async Task<Response<AttendLeaveReportResponse>> AttendAndLeaveReport(AttendLeaveReportInput input)
        {
            var response = new Response<AttendLeaveReportResponse>();
            try
            {
                if (input.EmployeeId == null || input.EmployeeId == 0)
                {
                    var employees = await _context.Employees.Include(x => x.WorkTimes).ThenInclude(i => i.OverTime).Include(x => x.Shift).ToListAsync();
                    if (employees.Count == 0)
                    {
                        response.status = false;
                        response.message = "لا يوجد موظفين";
                        return response;
                    }
                    foreach (var employee in employees)
                    {

                        if (employee == null)
                        {
                            continue;
                        }
                        var workTimes = employee.WorkTimes.Where(x => (x.WorkDate.Value >= input.From && x.WorkDate.Value <= input.To)).ToList();
                        var allowCome = employee.AllowCome;
                        foreach (var workTime in workTimes)
                        {
                            double? workHours = 0;
                            double? late = 0;
                            double? leaveEarly = 0;

                            if (workTime.WorkStart != null)
                            {
                                if (employee.Shift == null )
                                {
                                    late = null;
                                    leaveEarly = null;
                                }
                                else
                                {
                                    late = workTime.WorkStart.Value.Subtract(employee.Shift.TimeFrom.Value + (TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowCome??=0)))).TotalMinutes;
                                    if (late < 0)
                                    {
                                        late = 0;
                                    }
                                    //late = late * -1;
                                    //var x = employee.Shift.TimeTo.Value - (TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowOut.Value)));
                                    //var y = workTime.WorkEnd.Value;
                                    leaveEarly = employee.Shift.TimeTo?.Subtract( TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowOut??=0))).Subtract(workTime.WorkEnd.Value).TotalMinutes;
                                    if (leaveEarly < 0)
                                    {
                                        leaveEarly = 0;
                                    }
                                }
                                
                                if (workTime.WorkDate != null)
                                {

                                    workHours = Convert.ToDouble(string.Format("{0:0.0}", workTime.WorkEnd.Value.Subtract(workTime.WorkStart.Value).TotalHours));
                                }

                            }

                            var result = new AttendLeaveReportResponse
                            {
                                code = employee.Code,
                                date = workTime.WorkDate,
                                attendTime = workTime.WorkStart?.ToString(@"hh\:mm"),
                                leaveTime = workTime.WorkEnd?.ToString(@"hh\:mm"),
                                employeeName = employee.Name,
                                late = late,
                                overTime = workTime.OverTime?.OverTimeHours,
                                workHours = workHours,
                                leaveEarly = leaveEarly,
                                shiftName =  employee.Shift.ShiftName
                            };
                            response.data.Add(result);
                        }

                    }
                    response.status = true;
                    response.message = "تم سحب البيانات بنجاح";
                    return response;
                }
                else
                {

                    var employee = await _context.Employees.Include(x => x.WorkTimes).ThenInclude(i => i.OverTime).Include(x => x.Shift).Where(x => x.Id == input.EmployeeId).FirstOrDefaultAsync();
                    if (employee == null)
                    {
                        response.status = false;
                        response.message = "الموظف غير موجود";
                        return response;
                    }
                    var workTimes = employee.WorkTimes.Where(x => (x.WorkDate.Value >= input.From && x.WorkDate.Value <= input.To)).ToList();
                    var allowCome = employee.AllowCome;
                    foreach (var workTime in workTimes)
                    {
                        double? workHours = 0;
                        double? late = 0;
                        double? leaveEarly = 0;

                        if (workTime.WorkStart != null)
                        {
                            if (employee.Shift == null)
                            {
                                late = null;
                                leaveEarly = null;
                            }
                            else
                            {
                                late = workTime.WorkStart.Value.Subtract(employee.Shift.TimeFrom.Value + (TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowCome ??= 0)))).TotalMinutes;
                                if (late < 0)
                                {
                                    late = 0;
                                }
                                //late = late * -1;
                                //var x = employee.Shift.TimeTo.Value - (TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowOut.Value)));
                                //var y = workTime.WorkEnd.Value;
                                leaveEarly = employee.Shift.TimeTo?.Subtract(TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowOut ??= 0))).Subtract(workTime.WorkEnd.Value).TotalMinutes;
                                if (leaveEarly < 0)
                                {
                                    leaveEarly = 0;
                                }
                            }






                            //late = workTime.WorkStart.Value.Subtract(employee.Shift.TimeFrom.Value + (TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowCome.Value)))).TotalMinutes;
                            //if (late > 0)
                            //{
                            //    late = 0;
                            //}
                            //late = late * -1;
                            //var x = employee.Shift.TimeTo.Value - (TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowOut.Value)));
                            //var y = workTime.WorkEnd.Value;
                            //leaveEarly = (employee.Shift.TimeTo.Value - TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowOut.Value))).Subtract(workTime.WorkEnd.Value).TotalMinutes;
                            //if (leaveEarly < 0)
                            //{
                            //    leaveEarly = 0;
                            //}
                            //if (workTime.WorkDate != null)
                            //{

                            //    workHours = Convert.ToDouble(string.Format("{0:0.0}", workTime.WorkEnd.Value.Subtract(workTime.WorkStart.Value).TotalHours));
                            //}

                        }

                        var result = new AttendLeaveReportResponse
                        {
                            code = employee.Code,
                            date = workTime.WorkDate,
                            attendTime = workTime.WorkStart?.ToString(@"hh\:mm"),
                            leaveTime = workTime.WorkEnd?.ToString(@"hh\:mm"),
                            employeeName = employee.Name,
                            late = late,
                            overTime = workTime.OverTime?.OverTimeHours,
                            workHours = workHours,
                            leaveEarly = leaveEarly
                        };
                        response.data.Add(result);
                    }


                    response.status = true;
                    response.message = "تم سحب البيانات بنجاح";
                    return response;

                }
            }
            catch (System.Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }

        }
        public async Task<Response<ProductivitySalaryReportResponse>> ProductivitySalaryReport(ProductivitySalaryInput input)
        {
            var response = new Response<ProductivitySalaryReportResponse>();
            try
            {
                if (input.EmployeeId == null || input.EmployeeId == 0)
                {
                    var productivityEmployees = await _context.Employees.Include(x => x.ItemTransactions)
                        .Where(x => x.Productivity == true && x.SalaryTypeId == null)
                        .ToListAsync();

                    if (productivityEmployees.Count == 0)
                    {
                        response.status = false;
                        response.message = " لا يوجد عمال انتاجية";
                        return response;
                    }
                    foreach (var employee in productivityEmployees)
                    {
                        int? numOfItems = 0;
                        double? salary = 0;
                        var transactions = employee.ItemTransactions.Where(x => (x.TransDate?.Date >= input.From?.Date && x.TransDate?.Date <= input.To?.Date))?.ToList();
                        if (transactions.Count != 0)
                        {
                            foreach (var transaction in transactions)
                            {
                                salary += transaction?.ItemComissions;
                                numOfItems += transaction?.ItemQuantity;
                            }
                            response.data.Add(new ProductivitySalaryReportResponse
                            {
                                EmployeeId = employee.Id,
                                From = input.From,
                                To = input.To,
                                ItemsNum = numOfItems,
                                Salary = salary
                            });

                        }
                    }
                    response.status = true;
                    response.message = "تم سحب التقرير بنجاح";
                    return response;

                }
                else
                {
                    var employee = await _context.Employees.Include(x => x.ItemTransactions)
                .Where(x => x.Productivity == true && x.SalaryTypeId == null && x.Id == input.EmployeeId)
                .FirstOrDefaultAsync();

                    if (employee == null)
                    {
                        response.status = false;
                        response.message = "العامل غير موجود بالانتاجية او موجود بقسم الساعات و الانتاجيه معا";
                        return response;
                    }
                    int? numOfItems = 0;
                    double? salary = 0;
                    var transactions = employee.ItemTransactions.Where(x => (x.TransDate?.Date >= input.From?.Date && x.TransDate?.Date <= input.To?.Date))?.ToList();
                    if (transactions.Count != 0)
                    {
                        foreach (var transaction in transactions)
                        {
                            salary += transaction?.ItemComissions;
                            numOfItems += transaction?.ItemQuantity;
                        }
                        response.data.Add(new ProductivitySalaryReportResponse
                        {
                            EmployeeId = employee.Id,
                            EmployeeName = employee.Name,
                            From = input.From,
                            To = input.To,
                            ItemsNum = numOfItems,
                            Salary = salary
                        });
                        response.status = true;
                        response.message = "تم سحب تقرير العامل بنجاح";
                        return response;
                    }
                    else
                    {
                        response.status = false;
                        response.message = "لا يوجد عمليات لهذا العامل";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }
        public async Task<Response<SalaryReportResponse>> HoursAndProductivitySalary(ProductivitySalaryInput input)
        {
            var response = new Response<SalaryReportResponse>();
            try
            {
                if (input.From == null || input.To == null)
                {
                    response.status = false;
                    response.message = "يجب ملأ خانه التاريخ";
                    return response;
                }
                if (input.To < input.From)
                {
                    response.status = false;
                    response.message = "التاريخ -الى- يجب ان يكون اكبر من التاريخ -من- يجب تغييره ";
                    return response;
                }
                if (input.EmployeeId == null || input.EmployeeId == 0)
                {
                    var employees = await _context.Employees.Include(x => x.ItemTransactions)
                        .Include(x => x.WorkTimes)
                        .Include(x => x.Shift)
                        .Include(x => x.SalaryType)
                        .Include(x => x.Holiday)
                        .Where(x => x.SalaryTypeId != null)
                        .ToListAsync();

                    if (employees.Count == 0)
                    {
                        response.status = false;
                        response.message = "لا يوجد عمال ";
                        return response;
                    }
                    foreach (var employee in employees)
                    {
                        try
                        {
                            int? numOfItems = 0;
                            double? productivitySalary = 0;
                            //calculate productivity salary 
                            if (employee.Productivity != null && employee.Productivity.Value)
                            {
                                var transactions = employee.ItemTransactions.Where(x => (x.TransDate?.Date >= input.From?.Date && x.TransDate?.Date <= input.To?.Date))?.ToList();
                                if (transactions.Count != 0)
                                {
                                    foreach (var transaction in transactions)
                                    {
                                        productivitySalary += transaction?.ItemComissions;
                                        numOfItems += transaction?.ItemQuantity;
                                    }
                                }
                            }
                            var workHours = calculateHoursNum(employee, input.From.Value, input.To.Value);
                            var scheduledDays = calculateWorkDaysNum(employee, input.From.Value, input.To.Value);
                            var holidays = calculateHolidaysNum(employee, input.From.Value, input.To.Value);
                            var actualDays = calculateActualDaysNum(employee, input.From.Value, input.To.Value);
                            var absenceDays = scheduledDays - actualDays;
                            var dayPrice = calculateDayPrice(employee);
                            var salaryForDate = dayPrice * actualDays;

                            response.data.Add(new SalaryReportResponse
                            {
                                code = employee.Code,
                                SalaryType = employee.SalaryType.SalaryTypeName,
                                itemsNumber = numOfItems,
                                productivitySalary = Convert.ToDouble(string.Format("{0:0.0}", productivitySalary)),
                                workDays = actualDays,
                                scheduledDays = scheduledDays,
                                holidays = holidays,
                                absenceDays = absenceDays,
                                dayPrice = Convert.ToDouble(string.Format("{0:0.0}", dayPrice)),
                                workHours = Convert.ToDouble(string.Format("{0:0.0}", workHours)),
                                salaryForDate = Convert.ToDouble(string.Format("{0:0.0}", salaryForDate)),
                                baseSalary = Convert.ToDouble(employee.Salary),
                                dateFrom = input.From,
                                dateTo = input.To

                            });
                        }
                        catch (Exception)
                        {

                            continue;
                        }
                    }
                    response.status = true;
                    response.message = "تم سحب التقرير بنجاح";
                    return response;
                }
                else
                {
                    var employee = await _context.Employees.Include(x => x.ItemTransactions)
                         .Include(x => x.WorkTimes)
                         .Include(x => x.Shift)
                         .Include(x => x.SalaryType)
                         .Include(x => x.Holiday)
                         .Where(x => x.SalaryTypeId != null && x.Id == input.EmployeeId)
                         .FirstOrDefaultAsync();

                    if (employee == null)
                    {
                        response.status = false;
                        response.message = "لا يوجد عمال ";
                        return response;
                    }

                    try
                    {
                        int? numOfItems = 0;
                        double? productivitySalary = 0;
                        //calculate productivity salary 
                        if (employee.Productivity != null && employee.Productivity.Value)
                        {
                            var transactions = employee.ItemTransactions.Where(x => (x.TransDate?.Date >= input.From?.Date && x.TransDate?.Date <= input.To?.Date))?.ToList();
                            if (transactions.Count != 0)
                            {
                                foreach (var transaction in transactions)
                                {
                                    productivitySalary += transaction?.ItemComissions;
                                    numOfItems += transaction?.ItemQuantity;
                                }
                            }
                        }
                        var workHours = calculateHoursNum(employee, input.From.Value, input.To.Value);
                        var scheduledDays = calculateWorkDaysNum(employee, input.From.Value, input.To.Value);
                        var holidays = calculateHolidaysNum(employee, input.From.Value, input.To.Value);
                        var actualDays = calculateActualDaysNum(employee, input.From.Value, input.To.Value);
                        var absenceDays = scheduledDays - actualDays;
                        var dayPrice = calculateDayPrice(employee);
                        var salaryForDate = dayPrice * actualDays;

                        response.data.Add(new SalaryReportResponse
                        {
                            code = employee.Code,
                            SalaryType = employee.SalaryType.SalaryTypeName,
                            itemsNumber = numOfItems,
                            productivitySalary = Convert.ToDouble(string.Format("{0:0.0}", productivitySalary)),
                            workDays = actualDays,
                            scheduledDays = scheduledDays,
                            holidays = holidays,
                            absenceDays = absenceDays,
                            dayPrice = Convert.ToDouble(string.Format("{0:0.0}", dayPrice)),
                            workHours = Convert.ToDouble(string.Format("{0:0.0}", workHours)),
                            salaryForDate = Convert.ToDouble(string.Format("{0:0.0}", salaryForDate)),
                            baseSalary = Convert.ToDouble(employee.Salary),
                            dateFrom = input.From,
                            dateTo = input.To

                        });
                    }
                    catch (Exception ex)
                    {
                        response.status = false;
                        response.message = "  حدث خطا اثناء الحسابات   " + ex.Message;
                        return response;
                    }

                    response.status = true;
                    response.message = "تم سحب التقرير بنجاح";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }
        private double? calculateHoursNum(Employee emp, DateTime from, DateTime to)
        {
            if (emp.WorkTimes == null || emp.WorkTimes.Count == 0)
            {
                return 0;
            }
            double? hours = 0;
            var empWorkTimes = emp.WorkTimes.Where(x => (x.WorkDate >= from && x.WorkDate <= to)).ToList();
            foreach (var workTime in empWorkTimes)
            {
                if (workTime.WorkEnd == null || workTime.WorkStart == null)
                {
                    continue;
                }
                var workHours = workTime.WorkEnd?.Subtract(workTime.WorkStart.Value).TotalHours;
                if (workHours > emp.Shift.ShiftHour)
                {
                    workHours = emp.Shift.ShiftHour;
                }
                hours += workHours;
            }
            return hours;
        }
        private double? calculateActualDaysNum(Employee emp, DateTime from, DateTime to)
        {
            if (emp.WorkTimes == null || emp.WorkTimes.Count == 0)
            {
                return 0;
            }
            double? days = 0;
            var empWorkTimes = emp.WorkTimes.Where(x => (x.WorkDate >= from && x.WorkDate <= to)).ToList();
            foreach (var workTime in empWorkTimes)
            {

                days++;
            }
            return days;
        }
        private double? calculateWorkDaysNum(Employee emp, DateTime from, DateTime to)
        {
            if (emp.Holiday == null)
            {
                return 0;
            }
            //حساب ايام العمل المقرره
            List<string> holidaysName = new List<string>();
            foreach (PropertyInfo prop in emp.Holiday.GetType().GetProperties())
            {
                if (prop == typeof(Nullable))
                {
                    continue;
                }
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (type == typeof(bool))
                {

                    if ((bool)prop.GetValue(emp.Holiday, null))
                    {
                        var propName = prop.Name.ToUpper();
                        holidaysName.Add(propName);
                    }
                }
            }
            int WorkDays = 0;
            int HolidaysDays = 0;
            foreach (DateTime day in EachDay(from, to))
            {
                var dayName = day.DayOfWeek.ToString().ToUpper();
                if (holidaysName.Contains(dayName))
                {
                    HolidaysDays++;
                }
                else
                {
                    WorkDays++;
                }
            }

            return WorkDays;
        }
        private double? calculateHolidaysNum(Employee emp, DateTime from, DateTime to)
        {

            //حساب ايام العمل المقرره
            List<string> holidaysName = new List<string>();
            if (emp.Holiday == null)
            {
                return 0;
            }
            foreach (PropertyInfo prop in emp.Holiday.GetType().GetProperties())
            {
                if (prop == typeof(Nullable))
                {
                    continue;
                }
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (type == typeof(bool))
                {

                    if ((bool)prop.GetValue(emp.Holiday, null))
                    {
                        var propName = prop.Name.ToUpper();
                        holidaysName.Add(propName);
                    }
                }
            }
            int WorkDays = 0;
            int HolidaysDays = 0;
            foreach (DateTime day in EachDay(from, to))
            {
                var dayName = day.DayOfWeek.ToString().ToUpper();
                if (holidaysName.Contains(dayName))
                {
                    HolidaysDays++;
                }
                else
                {
                    WorkDays++;
                }
            }

            return HolidaysDays;
        }
        private double? calculateDayPrice(Employee emp)
        {
            double? numWorkDaysInWeek = 0;
            foreach (PropertyInfo prop in emp.Holiday.GetType().GetProperties())
            {
                if (prop == typeof(Nullable))
                {
                    continue;
                }
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (type == typeof(bool))
                {
                    if (!(bool)prop.GetValue(emp.Holiday, null))
                    {
                        numWorkDaysInWeek++;
                    }
                }
            }
            double? numOfHolidayDaysInWeek = 7 - numWorkDaysInWeek;
            double? numOfHolidayDaysInMonth = numOfHolidayDaysInWeek * 4;
            double? numOfWorkDaysInMonth = 30 - numOfHolidayDaysInMonth;
            if (numOfHolidayDaysInWeek == 7)
            {
                numOfWorkDaysInMonth = 0;
            }
            //Salary Type id = 1 ->>>> Month
            //Salary Type id = 2 ->>>> Week
            //Salary Type id = 3 ->>>> day
            double? daySalary;
            switch (emp.SalaryType.SalaryTypeId)
            {
                case 1:
                    {
                        daySalary = Convert.ToDouble(emp.Salary ??= 0) / numOfWorkDaysInMonth;
                        break;
                    }
                case 2:
                    {
                        daySalary = Convert.ToDouble(emp.Salary.Value) / numWorkDaysInWeek;
                        break;
                    }
                case 3:
                    {
                        daySalary = Convert.ToDouble(emp.Salary.Value);
                        break;
                    }
                default:
                    {
                        daySalary = Convert.ToDouble(emp.Salary.Value) / numOfWorkDaysInMonth;
                        break;
                    }

            }

            return daySalary;



        }
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }


    }
}