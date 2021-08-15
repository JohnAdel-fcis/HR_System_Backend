using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Interface
{

    public interface IEmployeeRepository
    {
        Task<Response<EmployeeResponse>> AddEmployee(EmployeeInput emp);
        Task<Response<EmployeeResponse>> EditEmployee(EmployeeResponse emp);
        Task<Response<EmployeeResponse>> GetAllEmployes();
        Task<Response<Employee>> GetAllDetailsEmployes();
        Task<Response<EmployeeResponse>> GetEmployee(int id);
        Task<Response<EmployeeResponse>> DeleteEmployee(int id);

    }
}
