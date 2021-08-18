using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HR_System_Backend.Model.Helper;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using zkemkeeper;
namespace HR_System_Backend.Repository.Repository
{
    public class FingerRepository : IFingerRepository
    {
        public async Task<Response<EmpInfoFinger>> GetLogsFromDevice(FingerGetAllInput input)
        {
            var response = GetLogs(input);
            if (!response.status)
            {
                return response ;
            }
            return null ;
       
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
                            idwEnrollNumber = sdwEnrollNumber,
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
    }
}