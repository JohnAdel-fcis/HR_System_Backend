using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Interface
{
    public interface IShiftRepository
    {
        Task<Response<ShiftResponse>> GetAllShifts();
        Task<Response<ShiftResponse>> GetShift(int id);
        Task<Response<ShiftResponse>> AddShift(ShiftInput shift);
        Task<Response<ShiftResponse>> EditShift(ShiftResponse shift);
        Task<Response<ShiftResponse>> DeleteShift(int id);
        Task<Response<OverTimeResponse>> AddOverTime(OverTimeInput input);


    }
}
