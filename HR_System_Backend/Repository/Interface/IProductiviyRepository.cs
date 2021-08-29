using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Interface
{
    public interface IProductiviyRepository
    {
        Task<Response<EmployeeResponse>> GetProductivityEmployees(); //

        Task<Response<ItemResponse>> GetEmployeeItems(int empId);

        Task<Response<ItemTransactionResponse>> GetTransactionsByDate(ProductivitySalaryInput input);

        Task<Response<ItemTransactionResponse>> GetAllEmployeeTransactions(int empId);

        Task<Response<ItemTransactionResponse>> AddTransaction(ItemTransactionInput input);

        Task<Response<ItemTransactionResponse>> EditTransaction(ItemTransactionResponse input);

        Task<Response<ItemTransactionResponse>> DeleteTransaction(int transactionId);

        Task<Response<ProductivitySalaryResponse>> CalculateSalary(ProductivitySalaryInput input);

        Task<Response<ItemTransactionResponse>> GetEmployeeTransactionsByDate(ProductivitySalaryInput input);

    }
}
