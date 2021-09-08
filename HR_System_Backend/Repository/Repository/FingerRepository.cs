using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Helper;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using System.Linq;
using zkemkeeper;
using Microsoft.EntityFrameworkCore;

namespace HR_System_Backend.Repository.Repository
{
    public class FingerRepository : IFingerRepository
    {

        private HR_DBContext _context;
        public FingerRepository(HR_DBContext context)
        {
            _context = context;
        }
        enum InOutMode
        {
            In,    // 0
            Out  // 1
        }

        public async Task<Response<EmpInfoFinger>> GetLogsFromDevice(FingerGetAllInput input)
        {
            var response = GetLogs(input);
            var data = response.data.OrderBy(x => x.LogDate);
            if (!response.status)
            {
                return response;
            }
            else
            {
                response.status = true;
                response.message = "تم حفظ البيانات بنجاح";
                response.data = data.ToList();
                return response;
            }

        }

        public async Task<Response<bool>> SaveLogsToDb(List<EmpInfoFinger> input)
        {
            var response = new Response<bool>();
            try
            {
                var data = input.OrderBy(x => x.LogDate);
                var CodeNoEmp = new List<int>();

                foreach (var item in data)
                {
                    var emp = _context.Employees.Include(x => x.Shift).Include(x => x.WorkTimes).Where(x => x.Code == item.idwEnrollNumber && x.DeviceId == item.deviceId).FirstOrDefault();
                    if (emp == null)
                    {
                        CodeNoEmp.Add(item.idwEnrollNumber);
                        continue;
                    }
                    if (emp.Shift != null)
                    {
                        var Shift = emp.Shift;
                        var InOutHours = (Convert.ToDecimal(Shift.TimeTo.Value.Subtract(Shift.TimeFrom.Value).TotalHours)) / 2;
                        var MiddleOfShift = Shift.TimeFrom.Value;
                        MiddleOfShift += TimeSpan.FromHours(Convert.ToDouble(InOutHours));
                        if (MiddleOfShift.Subtract(TimeSpan.Parse(item.LogTime)).TotalMilliseconds > 0)
                        {

                            item.idwInOutMode = (int)InOutMode.In;
                        }
                        else
                        {
                            item.idwInOutMode = (int)InOutMode.Out;
                        }
                    }
                    else
                    {
                        item.idwInOutMode = null;
                    }
                    emp.FingerLogs.Add(new FingerLog
                    {
                        Code = item.idwEnrollNumber,
                        LogDate = item.LogDate,
                        LogTime = TimeSpan.Parse(item.LogTime),
                        InOut = item.idwInOutMode
                    });
                    var date = item.LogDate;
                    var time = TimeSpan.Parse(item.LogTime);
                    var workTime = emp.WorkTimes.Where(x => x.WorkDate == date).FirstOrDefault();
                    // var test = _context.FingerLogs.Where(x => (x.LogDate == date && x.InOut == 0)).LastOrDefault();
                    if (workTime == null)
                    {
                        if (item.idwInOutMode == 0)
                        {
                            emp.WorkTimes.Add(new WorkTime
                            {
                                WorkDate = date,
                                WorkStart = time
                            });
                        }
                        else
                        {
                            emp.WorkTimes.Add(new WorkTime
                            {
                                WorkDate = date,
                                WorkEnd = time
                            });
                        }

                    }
                    else
                    {
                        if (item.idwInOutMode == 0)
                        {
                            workTime.WorkStart = time;
                        }
                        else
                        {
                            workTime.WorkEnd = time;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                var emps = _context.Employees.ToList();

                response.status = true;
                response.message = "تم سحب البيانات من الجهاز بنجاح";
                return response;
            }
            catch (Exception ex)
            {

                response.status = false;
                response.message = ex.Message;
                return response;
            }

        }

        public Response<bool> SetUserFinger(int userId, string name, int role, FingerGetAllInput input, string password = "")
        {
            var response = new Response<bool>();
            var device = new CZKEM();
            var connected = device.Connect_Net(input.ip, Convert.ToInt32(input.port));
            if (connected)
            {
                var added = device.SSR_SetUserInfo(1, userId.ToString(), name, password, role, true);
                if (added)
                {
                    response.status = true;
                    response.message = "Added succesfuly";
                    return response;
                }
                else
                {
                    response.status = false;
                    response.message = "Can't Add the User";
                    return response;
                }
            }
            else
            {
                response.status = false;
                response.message = "Can't connect the device";
                return response;
            }

        }

        public Response<bool> DeleteUserFinger(int machineNum, string code, Device input)
        {
            var response = new Response<bool>();
            var device = new CZKEM();
            var connected = device.Connect_Net(input.DeviceIp, Convert.ToInt32(input.DevicePort));
            if (connected)
            {
                var added = device.SSR_DeleteEnrollData(machineNum, code, 12);
                if (added)
                {
                    response.status = true;
                    response.message = "Deleted succesfuly";
                    return response;
                }
                else
                {
                    response.status = false;
                    response.message = "Can't Delete the User";
                    return response;
                }
            }
            else
            {
                response.status = false;
                response.message = "Can't connect the device";
                return response;
            }

        }

        public async Task<Response<GetUserInfoResponse>> GetUsersInfoFromDevice(FingerGetAllInput input)
        {
            var response = GetUsersInfo(input);
            try
            {
                if (!response.status)
                {
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات من الجهاز بنجاح";
                return response;
            }
            catch (Exception ex)
            {

                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }


        public async Task<Response<bool>> SaveUsersInfoToDb(List<GetUserInfoResponse> input)
        {
            var response = new Response<bool>();
            try
            {
                if (input.Count == 0)
                {
                    response.status = false;
                    response.message = "برجاء اضافه موظفين";
                    return response;
                }
                foreach (var item in input)
                {
                    var exist = _context.Employees.Where(x => x.Code == Int32.Parse(item.id) && x.DeviceId == item.deviceId).FirstOrDefault();
                    if (exist == null)
                    {
                        var emp = new Employee
                        {
                            Name = item.name,
                            Code = Int32.Parse(item.id),
                            DeviceId = item.deviceId,
                            Password = item.password,
                            RoleId = item.privilage
                        };
                        _context.Employees.Add(emp);
                    }
                    await _context.SaveChangesAsync();
                }
                response.status = true;
                response.message = "تم حفظ البيانات بنجاح";
                return response;
            }
            catch (Exception ex)
            {

                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }





        private Response<EmpInfoFinger> GetLogs(FingerGetAllInput input)
        {

            var response = new Response<EmpInfoFinger>();
            var axCZKEM1 = new zkemkeeper.CZKEM();



            bool bIsConnected = false;
            int iMachineNumber = 1;
            try
            {

                int idwErrorCode = 0;
                bIsConnected = axCZKEM1.Connect_Net(input.ip, Convert.ToInt32(input.port));
                if (bIsConnected == true)
                {

                    iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                    axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                }
                else
                {
                    axCZKEM1.GetLastError(ref idwErrorCode);
                }
                if (bIsConnected == false)
                {
                    response.status = false;
                    response.message = "Unable to connect the device";
                    return response;

                }
                var em = new EmployeeResponseReport();
                em.IsToday();
                string sdwEnrollNumber = "";
                int idwVerifyMode = 0;
                int idwInOutMode = 0;
                int idwYear = 0;
                int idwMonth = 0;
                int idwDay = 0;
                int idwHour = 0;
                int idwMinute = 0;
                int idwSecond = 0;
                int idwWorkcode = 0;
                int idwErrorCode2 = 0;
                var emps = _context.Employees;
                axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                List<EmpInfoFinger> userList = new List<EmpInfoFinger>();
                if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory

                {
                    while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                           out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                    {
                        var name = emps.Where(s => s.Code == Int32.Parse(sdwEnrollNumber)).FirstOrDefault().Name;
                        userList.Add(new EmpInfoFinger
                        {
                            idwEnrollNumber = Int32.Parse(sdwEnrollNumber),
                            idwVerifyMode = idwVerifyMode,
                            idwInOutMode = idwInOutMode,
                            LogDate = new DateTime(idwYear, idwMonth, idwDay).Date,
                            LogTime = new TimeSpan(idwHour, idwMinute, idwSecond).ToString(@"hh\:mm"),
                            name = name
                        });
                    }
                    axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                    response.status = true;
                    response.message = "تم سحب حضور الموظفين من جهاز البصمة بنجاح";
                    response.data = userList;
                    return response;
                }
                else
                {
                    axCZKEM1.GetLastError(ref idwErrorCode2);

                    if (idwErrorCode != 0)
                    {
                        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                        response.status = false;
                        response.message = "حدث خطا في قراءة البيانات من الجهاز" + idwErrorCode.ToString() + " Error";
                        return response;
                    }
                    else
                    {
                        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                        response.status = false;
                        response.message = "لا يوجد بيانات في الجهاز";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                response.status = false;
                response.message = "   حدث خطأ  " + ex.Message;
                return response;
            }
        }


        private Response<GetUserInfoResponse> GetUsersInfo(FingerGetAllInput input)
        {
            var response = new Response<GetUserInfoResponse>();
            var axCZKEM1 = new zkemkeeper.CZKEM();
            bool bIsConnected = false;
            int iMachineNumber = 1;
            try
            {

                int idwErrorCode = 0;
                bIsConnected = axCZKEM1.Connect_Net(input.ip, Convert.ToInt32(input.port));
                if (bIsConnected == true)
                {

                    iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                    axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                }
                else
                {
                    axCZKEM1.GetLastError(ref idwErrorCode);
                }
                if (bIsConnected == false)
                {
                    response.status = false;
                    response.message = "Unable to connect the device";
                    return response;

                }


                string id = string.Empty;
                string name = string.Empty;
                string password = string.Empty;
                int privilage = 0;
                bool enabled = false;
                axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                List<GetUserInfoResponse> userList = new List<GetUserInfoResponse>();
                while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out id, out name,
                              out password, out privilage, out enabled))//get records from the memory
                {
                    userList.Add(new GetUserInfoResponse
                    {
                        id = id,
                        name = name,
                        password = password,
                        privilage = privilage,
                        enabled = enabled

                    });
                }
                axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                if (userList.Count > 0)
                {
                    response.status = true;
                    response.message = "تم سحب حضور الموظفين من جهاز البصمة بنجاح";
                    response.data = userList;
                    return response;
                }
                else
                {
                    response.status = false;
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

        public async Task<Response<DeviceResponse>> GetAllDevices()
        {
            var response = new Response<DeviceResponse>();
            try
            {
                var devices = await _context.Devices.Select(x => new DeviceResponse { DeviceId = x.DeviceId, DeviceIp = x.DeviceIp, DevicePort = x.DevicePort, Priority = x.Priority , deviceName = x.DeviceName}).ToListAsync();
                if (devices.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد اجهزه";
                    return response;

                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = devices;
                return response;


            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<DeviceResponse>> AddDevice(DeviceInput input)
        {
            var response = new Response<DeviceResponse>();
            try
            {
                var device = await _context.Devices.Where(x => (x.DevicePort == input.devicePort && x.DeviceIp == input.deviceIP)).FirstOrDefaultAsync();
                if (device != null)
                {
                    response.status = false;
                    response.message = "هذا الجهاز موجود";
                    return response;
                }
                var newDevice = new Device { DeviceIp = input.deviceIP, DevicePort = input.devicePort, Priority = input.priority, DeviceName = input.deviceName };
                await _context.Devices.AddAsync(newDevice);
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تمت اضافة الجهاز بنجاح";
                response.data.Add(new DeviceResponse { DeviceIp = input.deviceIP, DevicePort = input.devicePort, Priority = input.priority, DeviceId = newDevice.DeviceId, deviceName = newDevice.DeviceName });
                return response;
            }
            catch (Exception)
            {
                response.status = false;
                response.message = "حدث خطأ";
                return response;
            }
        }

        public async Task<Response<DeviceResponse>> EditDevice(DeviceResponse input)
        {
            var respone = new Response<DeviceResponse>();
            try
            {
                var selectedDevice = await _context.Devices.Where(x => x.DeviceId == input.DeviceId).FirstOrDefaultAsync();
                if (selectedDevice == null)
                {
                    respone.status = false;
                    respone.message = "الجهاز غير موجود";
                    return respone;
                }
                selectedDevice.DeviceIp = input.DeviceIp;
                selectedDevice.DevicePort = input.DevicePort;
                selectedDevice.DeviceName = input.deviceName;
                await _context.SaveChangesAsync();
                respone.status = true;
                respone.message = "تم تعديل الجهاز بنجاح";
                respone.data.Add(input);
                return respone;
            }
            catch (Exception ex)
            {
                respone.status = false;
                respone.message = ex.Message;
                return respone;
            }





        }

        public async Task<Response<DeviceResponse>> GetDeviceByid(int id)
        {
            var response = new Response<DeviceResponse>();
            try
            {
                var device = await _context.Devices.Select(x => new DeviceResponse { DeviceId = x.DeviceId, DeviceIp = x.DeviceIp, DevicePort = x.DevicePort, Priority = x.Priority }).Where(x => x.DeviceId == id).FirstOrDefaultAsync();
                if (device == null)
                {
                    response.status = false;
                    response.message = "لا يوجد اجهزه";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data.Add(device);
                return response;
            }
            catch (Exception)
            {
                response.status = false;
                response.message = "حدث خطأ";
                return response;
            }

        }
    
        public async Task<Response<DeviceResponse>> DeleteDevice(int id)
        {
            var response = new Response<DeviceResponse>();
            try
            {
                var device = await _context.Devices.Where(x => x.DeviceId == id).FirstOrDefaultAsync();
                if (device == null)
                {
                    response.status = false;
                    response.message = "لا يوجد هذا الجهاز";
                    return response;
                }

                var employee = _context.Employees.Where(x => x.DeviceId == id).FirstOrDefault();
                if (employee!= null)
                {
                    response.status = false;
                    response.message = "يوجد موظفين مسجلين برقم الجهاز يرجي تغيرهم اولا";
                    return response;
                }
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم مسح الجهاز بنجاح";
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "حدث خطأ " + ex.Message ;
                return response;
            }
        }
    
    
    
    }
}
