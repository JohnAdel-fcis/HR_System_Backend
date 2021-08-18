using HR_System_Backend.Model;
using HR_System_Backend.Model.Helper;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Interface
{
    public interface IFingerRepository
    {
        Task<Response<EmpInfoFinger>> GetLogsFromDevice(FingerGetAllInput input);
    }
}