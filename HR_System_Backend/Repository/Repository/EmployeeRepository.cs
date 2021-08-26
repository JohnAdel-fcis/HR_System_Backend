using HR_System_Backend.Model;
using HR_System_Backend.Model.Helper;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        public async Task<Response<EmployeeResponse>> AddEmployee(EmployeeInput input)
        {
            var response = new Response<EmployeeResponse>();
            try
            {

                var resp = await ValidateEmployee(_context, input);
                if (!resp.status)
                    return resp;


                //Get The best code
                var codes = await _context.Employees.Where(s => s.DeviceId == input.deviceId).Select(x => x.Code).ToListAsync();
                codes.Sort();
                int newCode = 1;
                foreach (var code in codes)
                {
                    if (code != newCode)
                    {
                        break;
                    }
                    newCode++;
                }
                /////////////////////





                if (input.addToDevice)
                {
                    //Save The Employe To FingerPrint Device
                    var device = _context.Devices.Where(x => x.DeviceId == input.deviceId).FirstOrDefault();
                    var fingerRepo = new FingerRepository(_context);
                    var saveUserResponse = fingerRepo.SetUserFinger(newCode, input.name, input.roleId.Value, new FingerGetAllInput { ip = device.DeviceIp, port = device.DevicePort }, input.password);
                    if (saveUserResponse.status == false)
                    {
                        response.status = false;
                        response.message = saveUserResponse.message;
                        return response;
                    }
                    //////////////////////////////////////////
                }






                Holiday holiday = null;
                WorkDay workDays = null;
                if (input.holiday != null)
                {
                    holiday = new Holiday
                    {
                        Saturday = input.holiday.Saturday,
                        Sunday = input.holiday.Sunday,
                        Monday = input.holiday.Monday,
                        Tuesday = input.holiday.Tuesday,
                        Wednesday = input.holiday.Wednesday,
                        Thursday = input.holiday.Thursday,
                        Friday = input.holiday.Friday

                    };

                    workDays = new WorkDay
                    {
                        Saturday = !input.holiday.Saturday,
                        Sunday = !input.holiday.Sunday,
                        Monday = !input.holiday.Monday,
                        Tuesday = !input.holiday.Tuesday,
                        Wednesday = !input.holiday.Wednesday,
                        Thursday = !input.holiday.Thursday,
                        Friday = !input.holiday.Friday

                    };
                }
                //if (input.workDays != null)
                //{
                //    workDays = new WorkDay
                //    {
                //        Saturday = input.workDays.Saturday,
                //        Sunday = input.workDays.Sunday,
                //        Monday = input.workDays.Monday,
                //        Tuesday = input.workDays.Tuesday,
                //        Wednesday = input.workDays.Wednesday,
                //        Thursday = input.workDays.Thursday,
                //        Friday = input.workDays.Friday

                //    };
                //}
                var employee = new Employee
                {
                    Name = input.name,
                    Address = input.address,
                    Email = input.email,
                    CreateDate = input.createdDate,
                    Mobile = input.mobile,
                    Phone = input.phone,
                    Salary = input.salary,
                    TimeIn = TimeSpan.Parse(input.timeIn),
                    TimeOut = TimeSpan.Parse(input.timeOut),
                    AllowCome = input.allowCome,
                    AllowOut = input.allowOut,
                    BaseTime = input.baseTime,
                    DepartmentId = input.departmentId,
                    CategoryId = input.categoryId,
                    SalaryTypeId = input.salaryId,
                    ShiftId = input.shiftId,
                    Holiday = holiday,
                    WorkDay = workDays,
                    Code = newCode,
                    DeviceId = input.deviceId,
                    RoleId = input.roleId,
                    Productivity = input.productivity,
                    Password = input.password

                };
                // Check if employe poductivity
                if (input.productivity == true)
                {
                    if (input.items != null)
                    {
                        foreach (var item in input.items)
                        {
                            employee.Items.Add(new Item
                            {
                                ItemName = item.ItemName,
                                ItemPrice = item.ItemPrice,
                                ItemCommission = item.ItemCommission,
                                ItemQnty = item.ItemQnty
                            });
                        }
                    }
                }
                //////////////////////////////////////////

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                if (input.documents != null)
                {
                    if (input.documents.Count > 0)
                    {
                        var paths = SaveDocuments(input.documents, employee.Id);
                        foreach (var path in paths)
                        {
                            employee.Documents.Add(new Document
                            {
                                DocumentPath = path,
                                DocumentName = "FileName",
                                UploadDate = DateTime.Now,
                                AddedBy = "User"
                            });
                        }
                        await _context.SaveChangesAsync();
                    }

                }


                var employeeResponse = new EmployeeResponse
                {
                    id = employee.Id,
                    name = input.name,
                    address = input.address,
                    email = input.email,
                    createdDate = input.createdDate,
                    mobile = input.mobile,
                    phone = input.phone,
                    salary = input.salary,
                    timeIn = input.timeIn,
                    timeOut = input.timeOut,
                    allowCome = input.allowCome,
                    allowOut = input.allowOut,
                    baseTime = input.baseTime,
                    departmentId = input.departmentId,
                    categoryId = input.categoryId,
                    salaryId = input.salaryId,
                    shiftId = input.shiftId,
                    holiday = input.holiday
                };
                response.status = true;
                response.message = "تمت اضافة الموظف بنجاح";
                response.data.Add(employeeResponse);
                return response;

            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<EmployeeResponse>> DeleteEmployee(int id)
        {
            var fingerRepo = new FingerRepository(_context);
            var response = new Response<EmployeeResponse>();
            try
            {
                var emp = await _context.Employees.Include(s => s.WorkDay).Include(s => s.Holiday).Include(s => s.Documents).Where(x => x.Id == id).FirstOrDefaultAsync();
                if (emp == null)
                {
                    response.status = false;
                    response.message = "الموظف غير موجود";
                    return response;
                }
                var deleteFromDeviceResponse = fingerRepo.DeleteUserFinger(1, emp.Code.ToString(), emp.Device);
                if (!deleteFromDeviceResponse.status)
                {
                    response.status = false;
                    response.message = deleteFromDeviceResponse.message;
                    return response;
                }


                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();


                var exist = Directory.Exists("documents/" + id.ToString());
                if (exist)
                {
                    Directory.Delete("documents/" + id.ToString(), true);
                }





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

                var employee = _context.Employees.Include(s => s.Holiday)
                                                 .Include(s => s.WorkDay)
                                                 .Include(s => s.Items)
                                                 .Include(s => s.Documents)
                                                 .Where(x => x.Id == emp.id)
                                                 .FirstOrDefault();
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




                if (employee.Holiday == null)
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
                employee.Password = emp.password;
                employee.Productivity = emp.productivity;
                employee.RoleId = emp.roleId;

                if (emp.productivity == true)
                {
                    
                    if (emp.items != null)
                    {
                        foreach (var item in emp.items)
                        {
                            var itm = employee.Items.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                            if (itm == null)
                            {
                                employee.Items.Add(new Item
                                {
                                    ItemName = item.ItemName,
                                    ItemPrice = item.ItemPrice,
                                    ItemCommission = item.ItemCommission,
                                    ItemQnty = item.ItemQnty
                                });
                            }
                            itm.ItemName = item.ItemName;
                            itm.ItemPrice = item.ItemPrice;
                            itm.ItemCommission = item.ItemCommission;

                        }
                    }
                }




                await _context.SaveChangesAsync();
                //Delete employee doucuments folder
                var exist = Directory.Exists("documents/" + emp.id.ToString());
                if (exist)
                {
                    Directory.Delete("documents/" + emp.id.ToString(), true);
                }
                ////////////////////////////////////


                if (emp.documents != null)
                {
                    if (emp.documents.Count > 0)
                    {
                        var paths = SaveDocuments(emp.documents, employee.Id);
                        foreach (var path in paths)
                        {
                            employee.Documents.Add(new Document
                            {
                                DocumentPath = path,
                                DocumentName = "FileName",
                                UploadDate = DateTime.Now,
                                AddedBy = "User"
                            });
                        }
                        await _context.SaveChangesAsync();
                    }

                }

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

                var emplyee = await _context.Employees
                                        .Include(x => x.Holiday)
                                        .Include(x => x.WorkDay)
                                        .Include(x => x.Items)
                                        .Include(x => x.Documents)
                                        .Where(e => e.Id == id)
                                        .Select(x => new EmployeeResponse
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
                                            },
                                            deviceId = x.DeviceId,
                                            items = x.Items.Select(x => new ItemResponse { ItemId = x.ItemId, ItemName = x.ItemName, ItemPrice = x.ItemPrice, ItemCommission = x.ItemCommission }).ToList(),
                                            roleId = x.RoleId,
                                            password = x.Password,
                                            productivity = x.Productivity.Value
                                        }).FirstOrDefaultAsync();

                

                if (emplyee == null)
                {
                    response.status = true;
                    response.message = "لا يوجد بيانات";
                    return response;
                }

                var documentsPaths = await _context.Employees.Include(x => x.Documents).Where(e => e.Id == id).Select(x => x.Documents.Select(s => s.DocumentPath)).FirstOrDefaultAsync();
                var paths = documentsPaths.ToList();
                var imagesBase64 = ReadDocuments(paths);
                emplyee.documents = imagesBase64;



                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data.Add(emplyee);
                return response;

            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }

        }

        public async Task<Response<RoleResponse>> GetRoles()
        {
            var response = new Response<RoleResponse>();
            try
            {
                var roles = await _context.Roles.Select(x => new RoleResponse { RoleId = x.RoleId, RoleName = x.RoleName }).ToListAsync();
                if (roles.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد بيانات";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = roles;
                return response;

            }
            catch (Exception)
            {
                response.status = false;
                response.message = "حدث خطأ";
                return response;
            }
        }

        private async Task<Response<EmployeeResponse>> ValidateEmployee(HR_DBContext db, EmployeeInput emp)
        {
            var response = new Response<EmployeeResponse>();

            ///////////////////////////////////////////////////////////////////////////
            //check if  finger Device is existing
            ///////////////////////////////////////////////////////////////////////////
            ///
            if (emp.addToDevice)
            {
                var device = await db.Devices.Where(x => x.DeviceId == emp.deviceId).FirstOrDefaultAsync();
                if (device == null)
                {
                    response.status = false;
                    response.message = "جهاز البصمة غير موجود";
                    return response;
                }
            }


            ///////////////////////////////////////////////////////////////////////////
            //check if departmen is existing
            ///////////////////////////////////////////////////////////////////////////
            if (emp.departmentId != null)
            {
                var dept = await db.Departments.Where(x => x.DepartmentId == emp.departmentId).FirstOrDefaultAsync();
                if (dept == null)
                {
                    response.status = false;
                    response.message = "القسم غير موجود";
                    return response;
                }
            }



            ///////////////////////////////////////////////////////////////////////////
            //check if category is existing
            ///////////////////////////////////////////////////////////////////////////
            if (emp.categoryId != null)
            {
                var cat = await db.Categories.Where(x => x.CategoryId == emp.categoryId).FirstOrDefaultAsync();
                if (cat == null)
                {
                    response.status = false;
                    response.message = "نوع الوظيفه غير موجود";
                    return response;
                }

            }



            ///////////////////////////////////////////////////////////////////////////
            //check if salary type is existing
            ///////////////////////////////////////////////////////////////////////////
            if (emp.salaryId != null)
            {
                var salaryType = await db.SalaryTypes.Where(x => x.SalaryTypeId == emp.salaryId).FirstOrDefaultAsync();
                if (salaryType == null)
                {
                    response.status = false;
                    response.message = "نوع الراتب غير موجود";
                    return response;
                }
            }




            ///////////////////////////////////////////////////////////////////////////
            //check if shift is existing
            ///////////////////////////////////////////////////////////////////////////
            if (emp.shiftId != null)
            {
                var shift = await db.Shifts.Where(x => x.ShiftId == emp.shiftId).FirstOrDefaultAsync();
                if (shift == null)
                {
                    response.status = false;
                    response.message = "الوردية غير  موجوده";
                    return response;
                }
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
            if (emp.departmentId != null)
            {
                var dept = await db.Departments.Where(x => x.DepartmentId == emp.departmentId).FirstOrDefaultAsync();
                if (dept == null)
                {
                    response.status = false;
                    response.message = "القسم غير موجود";
                    return response;
                }
            }



            ///////////////////////////////////////////////////////////////////////////
            //check if category is existing
            ///////////////////////////////////////////////////////////////////////////
            if (emp.categoryId != null)
            {
                var cat = await db.Categories.Where(x => x.CategoryId == emp.categoryId).FirstOrDefaultAsync();
                if (cat == null)
                {
                    response.status = false;
                    response.message = "نوع الوظيفه غير موجود";
                    return response;
                }

            }



            ///////////////////////////////////////////////////////////////////////////
            //check if salary type is existing
            ///////////////////////////////////////////////////////////////////////////
            if (emp.salaryId != null)
            {
                var salaryType = await db.SalaryTypes.Where(x => x.SalaryTypeId == emp.salaryId).FirstOrDefaultAsync();
                if (salaryType == null)
                {
                    response.status = false;
                    response.message = "نوع الراتب غير موجود";
                    return response;
                }
            }




            ///////////////////////////////////////////////////////////////////////////
            //check if shift is existing
            ///////////////////////////////////////////////////////////////////////////
            if (emp.shiftId != null)
            {
                var shift = await db.Shifts.Where(x => x.ShiftId == emp.shiftId).FirstOrDefaultAsync();
                if (shift == null)
                {
                    response.status = false;
                    response.message = "الوردية غير  موجوده";
                    return response;
                }
            }


            response.status = true;
            return response;

        }

        private List<string> SaveDocuments(List<string> documents, int empId)
        {
            List<string> paths = new List<string>();
            foreach (var item in documents)
            {

                /////////////////// create Folder For each employee ///////////////////////////////////////////////////////////////////////
                string path = @"documents/" + empId.ToString();


                // Create directory temp1 if it doesn't exist
                Directory.CreateDirectory(path);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





                //////////////////////////*Generate id for each image*////////////////////////////////////////
                string newImageId;
                newImageId = Guid.NewGuid().ToString();
                path = path + "/" + newImageId + ".jpg";
                ////////////////////////////////////////////////////////////////////////////////////////////////



                /////////////////////////////////////////*** Convert base64 to image and save it ***//////////////////////////////////////
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    byte[] bytes = Convert.FromBase64String(item);
                    MemoryStream stream = new MemoryStream(bytes);
                    ImageConverter v = new ImageConverter();

                    //IFormFile file = new FormFile(stream, 0, bytes.Length, eqp.Name, eqp.Name);
                    IFormFile file = new FormFile(stream, 0, bytes.Length, "fileName", "fileName");

                    file.CopyTo(fileStream);
                }
                paths.Add(path);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            return paths;
        }

        private List<string> ReadDocuments(List<string> paths)
        {
            var images = new List<string>();
            foreach (var path in paths)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(path);
                var image = Convert.ToBase64String(imageArray);
                images.Add(image);
            }
            return images;
        }


    }
}
