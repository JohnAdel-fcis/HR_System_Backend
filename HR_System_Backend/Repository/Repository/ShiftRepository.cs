using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Repository
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly HR_DBContext _context;
        public ShiftRepository(HR_DBContext context)
        {
            _context = context;
        }

        public async Task<Response<OverTimeResponse>> AddOverTime(OverTimeInput input)
        {
            var response = new Response<OverTimeResponse>();
            try
            {
                var employee = await _context.Employees.Include(x => x.Holiday).Include(X => X.SalaryType).Include(x => x.WorkTimes).Where(x => x.Id == input.empId).FirstOrDefaultAsync();
                if (employee == null)
                {
                    response.status = false;
                    response.message = "الموظف غير موجود";
                    return response;
                }
                var overhourPrice = input.overHourPrice;
                if (input.overTimePercentage != 0 && input.overTimePercentage != null)
                {
                    if (employee.Productivity.Value && employee.SalaryType == null)
                    {
                        response.status = false;
                        response.message = "الموظف ليس له ساعات اضافيه لانه انتاجي";
                        return response;
                    }
                    if (!employee.Productivity.Value && employee.SalaryType == null)
                    {
                        response.status = false;
                        response.message = "يجب ادخال نوع الراتب للموظف بنظام الساعات";
                        return response;
                    }
                    else
                    {
                        var hourPrice = getHourPrice(employee);
                        overhourPrice = hourPrice * (input.overTimePercentage / 100);
                    }
                }
                var TotalOverTimePrice = overhourPrice * input.hours;
                var overTime = new OverTime
                {
                    OverTimeHours = input.hours.Value,
                    OverTimeDate = input.date,
                    OverHourPrice = input.overHourPrice,
                    OverTimePercentage = input.overTimePercentage,
                    Notes = input.note,
                    OverTimeTotal = TotalOverTimePrice
                };
                var workTime = employee.WorkTimes.Where(x => x.WorkDate == overTime.OverTimeDate).FirstOrDefault();
                if (workTime == null)
                {
                    employee.WorkTimes.Add(new WorkTime
                    {
                        WorkDate = overTime.OverTimeDate,
                        OverTime = overTime
                    });
                }
                else
                {
                    workTime.OverTime = overTime;
                }
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم اضافة العمل الإضافي";
                response.data.Add(new OverTimeResponse
                {
                    OverTimeId = overTime.OverTimeId,
                    OverHourPrice = overTime.OverHourPrice,
                    OverTimePercentage = overTime.OverTimePercentage,
                    OverTimeHours = overTime.OverTimeHours,
                    Notes = overTime.Notes,
                    OverTimeDate = overTime.OverTimeDate,
                    OverTimeTotal = overTime.OverTimeTotal

                });
                return response;
            }
            catch (System.Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<ShiftResponse>> AddShift(ShiftInput shift)
        {
            var response = new Response<ShiftResponse>();
            /*  if (shift.dateTo.Value.Subtract(shift.dateFrom.Value).Days <= 0)
             {
                 response.status = false;
                 response.message = "يجب ان يكون تاريخ الانتهاء اكبر من تاريخ البداية";
                 return response;
             } */
            var timeFrom = TimeSpan.Parse(shift.timeFrom);
            var timeTo = TimeSpan.Parse(shift.timeTo);
            var shiftHours = Convert.ToDouble(string.Format("{0:0.0}", timeTo.Subtract(timeFrom).TotalHours));

            var shft = new Shift
            {
                ShiftName = shift.shiftName,
                DateFrom = shift.dateFrom,
                DateTo = shift.dateTo,
                TimeFrom = timeFrom,
                TimeTo = timeTo,
                AllowCome = shift.allowCome,
                AllowLeave = shift.allowLeave,
                ShiftHour = shiftHours
            };
            try
            {
                _context.Shifts.Add(shft);
                await _context.SaveChangesAsync();


                response.status = true;
                response.message = "تم اضافة الوردية بنجاح";
                response.data.Add(new ShiftResponse
                {
                    shiftId = shft.ShiftId,
                    shiftName = shft.ShiftName,
                    dateFrom = shft.DateFrom,
                    dateTo = shft.DateTo,
                    timeFrom = timeFrom.ToString(@"hh\:mm"),
                    timeTo = timeTo.ToString(@"hh\:mm"),
                    allowCome = shift.allowCome,
                    allowLeave = shift.allowLeave,
                    shiftHour = shiftHours

                });

                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;

            }
        }
        public async Task<Response<ShiftResponse>> DeleteShift(int id)
        {
            var response = new Response<ShiftResponse>();
            try
            {
                var shift = _context.Shifts.Include(x=>x.Employees).Where(x => x.ShiftId == id).FirstOrDefault();
                if (shift == null)
                {
                    response.status = false;
                    response.message = "هذه الوردية غير موجوده";
                    return response;
                }
                if (shift.Employees.Count > 0)
                {
                    response.status = false;
                    response.message = "يوجد في الوردية موظفين ...قم بتغير وردياتهم اولا";
                    return response;
                }
                _context.Shifts.Remove(shift);
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم ازالة الموظف بنجاح";
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }
        public async Task<Response<ShiftResponse>> EditShift(ShiftResponse shift)
        {
            var response = new Response<ShiftResponse>();
            try
            {
                var shft = _context.Shifts.Where(x => x.ShiftId == shift.shiftId).FirstOrDefault();
                if (shft == null)
                {
                    response.status = false;
                    response.message = "هذه الورديه غير موجودة";
                    return response;
                }
                var timeFrom = TimeSpan.Parse(shift.timeFrom);
                var timeTo = TimeSpan.Parse(shift.timeTo);
                var shiftHours = Convert.ToDouble(string.Format("{0:0.0}", timeTo.Subtract(timeFrom).TotalHours));




                shft.ShiftName = shift.shiftName;
                shft.DateFrom = shift.dateFrom;
                shft.DateTo = shift.dateTo;
                shft.TimeFrom = timeFrom;
                shft.TimeTo = timeTo;
                shft.AllowCome = shift.allowCome;
                shft.AllowLeave = shift.allowLeave;
                shft.ShiftHour = shiftHours;
                await _context.SaveChangesAsync();


                response.status = true;
                response.message = "تم تعديل الوردية بنجاح";
                response.data.Add(new ShiftResponse
                {
                    shiftId = shft.ShiftId,
                    shiftName = shft.ShiftName,
                    dateFrom = shft.DateFrom,
                    dateTo = shft.DateTo,
                    timeFrom = shft.TimeFrom.Value.ToString(@"hh\:mm"),
                    timeTo = shft.TimeTo.Value.ToString(@"hh\:mm"),
                    allowCome = shift.allowCome,
                    allowLeave = shift.allowLeave,
                    shiftHour = shiftHours
                });

                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;

            }
        }
        public async Task<Response<ShiftResponse>> GetAllShifts()
        {
            var response = new Response<ShiftResponse>();
            try
            {
                var shifts = await _context.Shifts.Select(x => new ShiftResponse
                {
                    shiftId = x.ShiftId,
                    shiftName = x.ShiftName,
                    dateFrom = x.DateFrom,
                    dateTo = x.DateTo,
                    timeFrom = x.TimeFrom.Value.ToString(@"hh\:mm"),
                    timeTo = x.TimeTo.Value.ToString(@"hh\:mm"),
                    allowCome = x.AllowCome,
                    allowLeave = x.AllowLeave,
                    shiftHour = x.ShiftHour
                }).ToListAsync();
                if (shifts.Count > 0)
                {
                    response.status = true;
                    response.message = "تم سحب البيانات بنجاح ";
                    response.data = shifts;
                    return response;
                }
                else
                {
                    response.status = true;
                    response.message = "لا يوجد بيانات";
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
        public async Task<Response<ShiftResponse>> GetShift(int id)
        {
            var response = new Response<ShiftResponse>();
            try
            {
                var shift = await _context.Shifts.Where(s => s.ShiftId == id).Select(x => new ShiftResponse
                {
                    shiftId = x.ShiftId,
                    shiftName = x.ShiftName,
                    dateFrom = x.DateFrom,
                    dateTo = x.DateTo,
                    timeFrom = x.TimeFrom.Value.ToString(@"hh\:mm"),
                    timeTo = x.TimeTo.Value.ToString(@"hh\:mm"),
                    allowCome = x.AllowCome,
                    allowLeave = x.AllowLeave,
                    shiftHour = x.ShiftHour
                }).FirstOrDefaultAsync();
                if (shift != null)
                {
                    response.status = true;
                    response.message = "تم سحب البيانات بنجاح ";
                    response.data.Add(shift);
                    return response;
                }
                else
                {
                    response.status = true;
                    response.message = "لا يوجد بيانات";
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

        private double getHourPrice(Employee emp)
        {
            var NumWorkDaysInWeek = 0;
            var holiday = emp.Holiday;
            foreach (PropertyInfo prop in holiday.GetType().GetProperties())
            {
                if (prop == typeof(Nullable))
                {
                    continue;
                }
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (type == typeof(bool))
                {
                    if (!(bool)prop.GetValue(holiday, null))
                    {
                        NumWorkDaysInWeek++;
                    }
                }
            }
            if (emp.SalaryType.SalaryTypeName == "شهري")
            {
                var numWorkDayInMonth = NumWorkDaysInWeek * 4;
                var baseSalary = emp.Salary;
                var dayPrice = baseSalary / numWorkDayInMonth;
                var numWorkingHour = emp.BaseTime;
                var hourPrice = Convert.ToDouble(dayPrice / numWorkingHour);
                return hourPrice;
            }
            else if (emp.SalaryType.SalaryTypeName == "أسبوعي")
            {

                var baseSalary = emp.Salary;
                var dayPrice = baseSalary / NumWorkDaysInWeek;
                var numWorkingHour = emp.BaseTime;
                var hourPrice = Convert.ToDouble(dayPrice / numWorkingHour);
                return hourPrice;
            }
            else if (emp.SalaryType.SalaryTypeName == "يومي")
            {
                var baseSalary = emp.Salary;
                var dayPrice = baseSalary;
                var numWorkingHour = emp.BaseTime;
                var hourPrice = Convert.ToDouble(dayPrice / numWorkingHour);
                return hourPrice;
            }

            return 0;
        }
    }
}
