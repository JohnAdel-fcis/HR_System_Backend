using HR_System_Backend.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Interface
{
    public interface IBranchRepository
    {
        Task<Response<String>> AddBranch(string name);
        Task<Response<String>> EditBranch(BranchResponse input);
        Task<Response<String>> DeleteBranch(BranchResponse input);
        Task<Response<EmployeeResponse>> GetAllEmployeesInBranch(int branchId);
        Task<Response<DeviceResponse>> GetAllDevicesInBranch(int branchId);
        Task<Response<BranchResponse>> GetAllBranches();
        Task<Response<BranchResponse>> GetBranch(int id);

    }
}
