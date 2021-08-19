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
namespace HR_System_Backend.Repository.Repository
{
    public class FingerRepository : IFingerRepository
    {

        private HR_DBContext _context;
        public FingerRepository(HR_DBContext context)
        {
            _context = context;
        }


        public async Task<Response<EmpInfoFinger>> GetLogsFromDevice(FingerGetAllInput input)
        {
            var response = GetLogs(input);
            if (!response.status)
            {
                return response;
            }
            try
            {
                var data = response.data;
                var CodeNoEmp = new List<int>();
                var Old = new EmpInfoFinger();
                foreach (var item in data)
                {
                    var emp = _context.Employees.Where(x => x.Code == item.idwEnrollNumber).FirstOrDefault();
                    if (emp == null)
                    {
                        CodeNoEmp.Add(item.idwEnrollNumber);
                        continue;
                    }
                    emp.FingerLogs.Add(new FingerLog
                    {
                        Code = item.idwEnrollNumber,
                        LogDate = new DateTime(item.year, item.month, item.day),
                        LogTime = new TimeSpan(item.hour, item.minute, item.second),
                        InOut = item.idwInOutMode
                    });
                    var date = new DateTime(item.year, item.month, item.day);
                    var timeIn = new TimeSpan(item.hour, item.minute, item.second);
                    var workTime = emp.WorkTimes.Where(x => x.WorkDate == date).FirstOrDefault();

                    if (workTime == null)
                    {
                        if (item.idwInOutMode == 0)
                        {
                            emp.WorkTimes.Add(new WorkTime
                            {
                                WorkDate = date,
                                WorkStart = timeIn
                            });
                        }
                        else
                        {
                            emp.WorkTimes.Add(new WorkTime
                            {
                                WorkDate = date,
                                WorkEnd = timeIn
                            });
                        }

                    }
                    else
                    {
                        if (item.idwInOutMode == 0)
                        {
                            workTime.WorkStart = timeIn;
                        }
                        else
                        {
                            workTime.WorkEnd = timeIn;
                        }
                    }






















                }
                await _context.SaveChangesAsync();
                response.status = true ;
                response.message="تم حفظ البيانات بنجاح";
                return response ;
            }
            catch (Exception ex)
            {

               response.status= false ; 
               response.message = ex.Message ;
               return response ;
            }

        }




        public Response<bool> SetUserFinger(int userId, string name, FingerGetAllInput input, string password = "")
        {
            var response = new Response<bool>();
            var device = new CZKEM();
            var connected = device.Connect_Net(input.ip, Convert.ToInt32(input.port));
            if (connected)
            {
                var added = device.SSR_SetUserInfo(1, userId.ToString(), name, password, 1, true);
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





        public async Task<Response<GetUserInfoResponse>> GetUsersInfoFromDevice(FingerGetAllInput input)
        {
            var response = GetUsersInfo(input);
            try
            {
                if (!response.status)
                {
                    return response;
                }
                foreach (var item in response.data)
                {
                    var exist = _context.Employees.Where(x => x.Code == Int32.Parse(item.id)).FirstOrDefault();
                    if (exist != null)
                    {
                        var emp = new Employee
                        {
                            Name = item.name,
                            Code = Int32.Parse(item.id)
                        };
                        _context.Employees.Add(emp);
                    }
                }
                await _context.SaveChangesAsync();
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



                axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                List<EmpInfoFinger> userList = new List<EmpInfoFinger>();
                if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory

                {
                    while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                               out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                    {
                        userList.Add(new EmpInfoFinger
                        {
                            idwEnrollNumber = Int32.Parse(sdwEnrollNumber),
                            idwVerifyMode = idwVerifyMode,
                            idwInOutMode = idwInOutMode,
                            year = idwYear,
                            month = idwMonth,
                            day = idwDay,
                            hour = idwHour,
                            minute = idwMinute,
                            second = idwSecond
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

    }
}
