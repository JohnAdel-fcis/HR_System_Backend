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
        Response<bool> SetUserFinger(int userId, string name, FingerGetAllInput input, string password = null);

        Task<Response<GetUserInfoResponse>> GetUsersInfoFromDevice(FingerGetAllInput input);
    }
}