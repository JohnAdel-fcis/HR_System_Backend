using HR_System_Backend.Model;
using HR_System_Backend.Model.Helper;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private HR_DBContext _context;
        public EmployeeRepository(HR_DBContext context)
        {
            _context = context;
        }

        public async Task<Response<EmployeeResponse>> AddEmployee(EmployeeInput emp)
        {
            var response = new Response<EmployeeResponse>();
            try
            {

                var resp = await ValidateEmployee(_context, emp);
                if (!resp.status)
                    return resp;
                var holiday = new Holiday
                {
                    Saturday = emp.holiday.Saturday,
                    Sunday = emp.holiday.Sunday,
                    Monday = emp.holiday.Monday,
                    Tuesday = emp.holiday.Tuesday,
                    Wednesday = emp.holiday.Wednesday,
                    Thursday = emp.holiday.Thursday,
                    Friday = emp.holiday.Friday

                };
                var workDays = new WorkDay
                {
                    Saturday = emp.workDays.Saturday,
                    Sunday = emp.workDays.Sunday,
                    Monday = emp.workDays.Monday,
                    Tuesday = emp.workDays.Tuesday,
                    Wednesday = emp.workDays.Wednesday,
                    Thursday = emp.workDays.Thursday,
                    Friday = emp.workDays.Friday

                };
                var employee = new Employee
                {
                    Name = emp.name,
                    Address = emp.address,
                    Email = emp.email,
                    CreateDate = emp.createdDate,
                    Mobile = emp.mobile,
                    Phone = emp.phone,
                    Salary = emp.salary,
                    TimeIn = TimeSpan.Parse(emp.timeIn),
                    TimeOut = TimeSpan.Parse(emp.timeOut),
                    AllowCome = emp.allowCome,
                    AllowOut = emp.allowOut,
                    BaseTime = emp.baseTime,
                    DepartmentId = emp.departmentId,
                    CategoryId = emp.categoryId,
                    SalaryTypeId = emp.salaryId,
                    ShiftId = emp.shiftId,
                    Holiday = holiday,
                    WorkDay = workDays
                };

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                var employeeResponse = new EmployeeResponse
                {
                    id = employee.Id,
                    name = emp.name,
                    address = emp.address,
                    email = emp.email,
                    createdDate = emp.createdDate,
                    mobile = emp.mobile,
                    phone = emp.phone,
                    salary = emp.salary,
                    timeIn = emp.timeIn,
                    timeOut = emp.timeOut,
                    allowCome = emp.allowCome,
                    allowOut = emp.allowOut,
                    baseTime = emp.baseTime,
                    departmentId = emp.departmentId,
                    categoryId = emp.categoryId,
                    salaryId = emp.salaryId,
                    shiftId = emp.shiftId,
                    holiday = emp.holiday,
                    workDays = emp.workDays


                };
                response.status = true;
                response.message = "تمت اضافة الموظف بنجاح";
                response.data.Add(employeeResponse);
                return response;

            }
            catch (Exception)
            {
                response.status = false;
                response.message = "حدث خطأ";
                return response;
            }
        }

        public async Task<Response<EmployeeResponse>> DeleteEmployee(int id)
        {
            var response = new Response<EmployeeResponse>();
            try
            {
                var emp = await _context.Employees.Include(s => s.WorkDay).Include(s => s.Holiday).Where(x => x.Id == id).FirstOrDefaultAsync();
                if (emp == null)
                {
                    response.status = false;
                    response.message = "الموظف غير موجود";
                    return response;
                }
                _context.Employees.Remove(emp);
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

        public async Task<Response<EmployeeResponse>> EditEmployee(EmployeeResponse emp)
        {
            var response = new Response<EmployeeResponse>();
            try
            {

                var employee = _context.Employees.Include(s => s.Holiday).Include(s => s.WorkDay).Where(x => x.Id == emp.id).FirstOrDefault();
                if (employee == null)
                {
                    response.status = false;
                    response.message = "الموظف غير موجود";
                    return response;
                }



                var resp = await ValidateEmployee(_context, emp);
                if (!resp.status)
                {
                    return resp;
                }
                if (employee.Holiday==null)
                {
                    employee.Holiday = new Holiday();
                }
                if (employee.WorkDay == null)
                {
                    employee.WorkDay = new WorkDay();
                }

                employee.Holiday.Saturday = emp.holiday.Saturday;
                employee.Holiday.Sunday = emp.holiday.Sunday;
                employee.Holiday.Monday = emp.holiday.Monday;
                employee.Holiday.Tuesday = emp.holiday.Tuesday;
                employee.Holiday.Wednesday = emp.holiday.Wednesday;
                employee.Holiday.Thursday = emp.holiday.Thursday;
                employee.Holiday.Friday = emp.holiday.Friday;


                employee.WorkDay.Sunday = emp.workDays.Sunday;
                employee.WorkDay.Monday = emp.workDays.Monday;
                employee.WorkDay.Tuesday = emp.workDays.Tuesday;
                employee.WorkDay.Wednesday = emp.workDays.Wednesday;
                employee.WorkDay.Thursday = emp.workDays.Thursday;
                employee.WorkDay.Friday = emp.workDays.Friday;


                employee.Id = emp.id;
                employee.Name = emp.name;
                employee.Address = emp.address;
                employee.Email = emp.email;
                employee.CreateDate = emp.createdDate;
                employee.Mobile = emp.mobile;
                employee.Phone = emp.phone;
                employee.Salary = emp.salary;
                employee.TimeIn = TimeSpan.Parse(emp.timeIn);
                employee.TimeOut = TimeSpan.Parse(emp.timeOut);
                employee.AllowCome = emp.allowCome;
                employee.AllowOut = emp.allowOut;
                employee.BaseTime = emp.baseTime;
                employee.DepartmentId = emp.departmentId;
                employee.CategoryId = emp.categoryId;
                employee.SalaryTypeId = emp.salaryId;
                employee.ShiftId = emp.shiftId;


                await _context.SaveChangesAsync();

                response.status = true;
                response.message = "تم تعديل الموظف بنجاح";
                response.data.Add(emp);

                return response;

            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<Employee>> GetAllDetailsEmployes()
        {
            var response = new Response<Employee>();
            try
            {

                var emplyees = await _context.Employees.Include(x => x.Holiday).Include(x => x.WorkTimes).ToListAsync();

                if (emplyees.Count == 0)
                {
                    response.status = true;
                    response.message = "لا يوجد بيانات";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = emplyees;
                return response;

            }
            catch (Exception ex)
            {
                response.status = true;
                response.message = ex.Message;
                return response;
            }

        }

        public async Task<Response<EmployeeResponse>> GetAllEmployes()
        {
            var response = new Response<EmployeeResponse>();
            try
            {

                var emplyees = await _context.Employees.Include(x => x.Holiday).Include(x => x.WorkTimes).Select(x => new EmployeeResponse
                {
                    id = x.Id,
                    name = x.Name,
                    address = x.Address,
                    email = x.Email,
                    phone = x.Phone,
                    mobile = x.Mobile,
                    salary = x.Salary,
                    salaryId = x.SalaryTypeId,
                    categoryId = x.CategoryId,
                    shiftId = x.ShiftId,
                    allowCome = x.AllowCome,
                    allowOut = x.AllowOut,
                    timeIn = x.TimeIn.Value.ToString(@"hh\:mm"),
                    timeOut = x.TimeOut.Value.ToString(@"hh\:mm"),
                    baseTime = x.BaseTime,
                    createdDate = x.CreateDate,
                    departmentId = x.DepartmentId,
                    holiday = new Week
                    {
                        Saturday = x.Holiday.Saturday,
                        Sunday = x.Holiday.Sunday,
                        Monday = x.Holiday.Monday,
                        Tuesday = x.Holiday.Tuesday,
                        Wednesday = x.Holiday.Wednesday,
                        Thursday = x.Holiday.Thursday,
                        Friday = x.Holiday.Friday
                    },
                    workDays = new Week
                    {
                        Saturday = x.WorkDay.Saturday,
                        Sunday = x.WorkDay.Sunday,
                        Monday = x.WorkDay.Monday,
                        Tuesday = x.WorkDay.Tuesday,
                        Wednesday = x.WorkDay.Wednesday,
                        Thursday = x.WorkDay.Thursday,
                        Friday = x.WorkDay.Friday
                    }
                }).ToListAsync();

                if (emplyees.Count == 0)
                {
                    response.status = true;
                    response.message = "لا يوجد بيانات";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = emplyees;
                return response;

            }
            catch (Exception ex)
            {
                response.status = true;
                response.message = ex.Message;
                return response;
            }

        }

        public async Task<Response<EmployeeResponse>> GetEmployee(int id)
        {
            var response = new Response<EmployeeResponse>();
            try
            {

                var emplyees = await _context.Employees.Where(e => e.Id == id).Select(x => new EmployeeResponse
                {
                    id = x.Id,
                    name = x.Name,
                    address = x.Address,
                    email = x.Email,
                    phone = x.Phone,
                    mobile = x.Mobile,
                    salary = x.Salary,
                    salaryId = x.SalaryTypeId,
                    categoryId = x.CategoryId,
                    shiftId = x.ShiftId,
                    allowCome = x.AllowCome,
                    allowOut = x.AllowOut,
                    timeIn = x.TimeIn.Value.ToString(@"hh\:mm"),
                    timeOut = x.TimeOut.Value.ToString(@"hh\:mm"),
                    baseTime = x.BaseTime,
                    createdDate = x.CreateDate,
                    departmentId = x.DepartmentId
                }).ToListAsync();

                if (emplyees.Count == 0)
                {
                    response.status = true;
                    response.message = "لا يوجد بيانات";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = emplyees;
                return response;

            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }

        }




        private async Task<Response<EmployeeResponse>> ValidateEmployee(HR_DBContext db, EmployeeInput emp)
        {
            var response = new Response<EmployeeResponse>();




            ///////////////////////////////////////////////////////////////////////////
            //check if departmen is existing
            ///////////////////////////////////////////////////////////////////////////
            var dept = await db.Departments.Where(x => x.DepartmentId == emp.departmentId).FirstOrDefaultAsync();
            if (dept == null)
            {
                response.status = false;
                response.message = "القسم غير موجود";
                return response;
            }


            ///////////////////////////////////////////////////////////////////////////
            //check if category is existing
            ///////////////////////////////////////////////////////////////////////////
            var cat = await db.Categories.Where(x => x.CategoryId == emp.categoryId).FirstOrDefaultAsync();
            if (cat == null)
            {
                response.status = false;
                response.message = "نوع الوظيفه غير موجود";
                return response;
            }



            ///////////////////////////////////////////////////////////////////////////
            //check if salary type is existing
            ///////////////////////////////////////////////////////////////////////////
            ///
            var salaryType = await db.SalaryTypes.Where(x => x.SalaryTypeId == emp.salaryId).FirstOrDefaultAsync();
            if (salaryType == null)
            {
                response.status = false;
                response.message = "نوع الراتب غير موجود";
                return response;
            }



            ///////////////////////////////////////////////////////////////////////////
            //check if shift is existing
            ///////////////////////////////////////////////////////////////////////////
            var shift = await db.Shifts.Where(x => x.ShiftId == emp.shiftId).FirstOrDefaultAsync();
            if (shift == null)
            {
                response.status = false;
                response.message = "الوردية غير  موجوده";
                return response;
            }

            response.status = true;
            return response;

        }
        private async Task<Response<EmployeeResponse>> ValidateEmployee(HR_DBContext db, EmployeeResponse emp)
        {
            var response = new Response<EmployeeResponse>();





            ///////////////////////////////////////////////////////////////////////////
            //check if departmen is existing
            ///////////////////////////////////////////////////////////////////////////
            var dept = await db.Departments.Where(x => x.DepartmentId == emp.departmentId).FirstOrDefaultAsync();
            if (dept == null)
            {
                response.status = false;
                response.message = "القسم غير موجود";
                return response;
            }


            ///////////////////////////////////////////////////////////////////////////
            //check if category is existing
            ///////////////////////////////////////////////////////////////////////////
            var cat = await db.Categories.Where(x => x.CategoryId == emp.categoryId).FirstOrDefaultAsync();
            if (cat == null)
            {
                response.status = false;
                response.message = "نوع الوظيفه غير موجود";
                return response;
            }



            ///////////////////////////////////////////////////////////////////////////
            //check if salary type is existing
            ///////////////////////////////////////////////////////////////////////////
            ///
            var salaryType = await db.SalaryTypes.Where(x => x.SalaryTypeId == emp.salaryId).FirstOrDefaultAsync();
            if (salaryType == null)
            {
                response.status = false;
                response.message = "نوع الراتب غير موجود";
                return response;
            }



            ///////////////////////////////////////////////////////////////////////////
            //check if shift is existing
            ///////////////////////////////////////////////////////////////////////////
            var shift = await db.Shifts.Where(x => x.ShiftId == emp.shiftId).FirstOrDefaultAsync();
            if (shift == null)
            {
                response.status = false;
                response.message = "الوردية غير  موجوده";
                return response;
            }
            response.status = true;
            return response;

        }

    }
}
