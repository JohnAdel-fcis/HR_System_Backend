using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using System.Linq;
using HR_System_Backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;

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
                var employee = await _context.Employees.Include(x => x.WorkTimes).ThenInclude(i => i.OverTime).Include(x => x.Shift).Where(x => x.Id == input.EmployeeId).FirstOrDefaultAsync();
                if (employee == null)
                {
                    response.status = false;
                    response.message = "الموظف غير موجود";
                    return response;
                }
                var workTimes = employee.WorkTimes.Where(x => (x.WorkDate.Value >= input.From && x.WorkDate.Value >= input.From)).ToList();
                var allowCome = employee.AllowCome;
                foreach (var workTime in workTimes)
                {
                    double workHours = 0;
                    double late = 0;


                    if (workTime.WorkStart != null)
                    {

                        late = workTime.WorkStart.Value.Subtract(employee.Shift.TimeFrom.Value + (TimeSpan.FromMinutes(Convert.ToDouble(employee.AllowCome.Value)))).TotalMinutes;
                        if (late <= 0)
                        {
                            late = 0;
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
                        workHours = workHours
                    };
                    response.data.Add(result);
                }


                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                return response;


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
                if (input.EmployeeId == null)
                {
                    var productivityEmployees = await _context.Employees.Include(x => x.ItemTransactions)
                        .Where(x => x.Productivity == true && x.SalaryTypeId == null)
                        .ToListAsync();

                    if (productivityEmployees.Count == 0)
                    {
                        response.status = false;
                        response.message = "لا يوجد عمال انتاجية";
                        return response;
                    }
                    foreach (var employee in productivityEmployees)
                    {
                        int? numOfItems = 0;
                        double? salary = 0;
                        var transactions = employee.ItemTransactions.Where(x=>(x.TransDate?.Date >= input.From?.Date && x.TransDate?.Date>=input.To?.Date)   )?.ToList();
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
                        response.message = "العامل غير موجود بالانتاجية";
                        return response;
                    }
                    int? numOfItems = 0;
                    double? salary = 0;
                    var transactions = employee.ItemTransactions.Where(x => (x.TransDate?.Date >= input.From?.Date && x.TransDate?.Date >= input.To?.Date))?.ToList();
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




    }
}