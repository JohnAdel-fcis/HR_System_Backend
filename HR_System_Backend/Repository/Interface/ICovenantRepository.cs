using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Interface
{
    interface ICovenantRepository
    {
        Task<Response<bool>> AddCovenant(CovenantInput input);
        Task<Response<CovenantResponse>> EditCovenant(CovenantResponse input);
        Task<Response<CovenantResponse>> DeleteCovenant(int coventantId);
        Task<Response<CovenantResponse>> GetAllCovenant();
        Task<Response<CovenantResponse>> GetCovenantById(int covenantId);


    }
}
