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
        Task<Response<DeviceResponse>> GetAllDevices();
        Task<Response<EmpInfoFinger>> GetLogsFromDevice(FingerGetAllInput input);
        Response<bool> SetUserFinger(int userId, string name, int role, FingerGetAllInput input, string password = null);
        Task<Response<bool>> SaveLogsToDb(List<EmpInfoFinger> input);
        Task<Response<bool>> SaveUsersInfoToDb(List<GetUserInfoResponse> input);
        Task<Response<GetUserInfoResponse>> GetUsersInfoFromDevice(FingerGetAllInput input);
        Response<bool> DeleteUserFinger(int machineNum, string code, Device input);
        Task<Response<DeviceResponse>> AddDevice(DeviceInput input);
        Task<Response<DeviceResponse>> EditDevice(DeviceResponse input);
        Task<Response<DeviceResponse>> GetDeviceByid(int id);

    }
}