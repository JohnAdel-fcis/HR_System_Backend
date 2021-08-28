using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Repository
{

    public class ProductiviyRepository : IProductiviyRepository
    {
        private HR_DBContext _context;
        public ProductiviyRepository(HR_DBContext context)
        {
            _context = context;
        }

        public async Task<Response<ItemTransactionResponse>> AddTransaction(ItemTransactionInput input)
        {
            var response = new Response<ItemTransactionResponse>();
            try
            {
                var employee = await _context.Employees.Where(x => x.Id == input.EmployeeId).FirstOrDefaultAsync();
                if (employee == null || !employee.Productivity.Value)
                {
                    response.status = false;
                    response.message = "الموظف غير موجود او ليس في قسم الانتاجية";
                    return response;
                }
                var item = await _context.Items.Where(x => x.ItemId == input.ItemId).FirstOrDefaultAsync();
                if (item == null)
                {
                    response.status = false;
                    response.message = "نوع القطعه غير موجود ";
                    return response;
                }

                var commisionPerUnit = item.ItemPrice / item.ItemQnty;


                var transaction = new ItemTransaction
                {
                    ItemId = input.ItemId,
                    ItemQuantity = input.Quantity,
                    ItemComissions = (commisionPerUnit * input.Quantity),
                    TransDate = input.Date
                };
                employee.ItemTransactions.Add(transaction);
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم اضافة العمليه بنجاح";
                response.data.Add(new ItemTransactionResponse
                {
                    EmpId = input.EmployeeId,
                    ItemId = input.ItemId,
                    TarnsId = transaction.TarnsId,
                    ItemComissions = transaction.ItemComissions,
                    ItemQuantity = transaction.ItemQuantity,
                    TransDate = transaction.TransDate
                });
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<ProductivitySalaryResponse>> CalculateSalary(ProductivitySalaryInput input)
        {
            var response = new Response<ProductivitySalaryResponse>();
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<ItemTransactionResponse>> DeleteTransaction(int transactionId)
        {
            var response = new Response<ItemTransactionResponse>();
            try
            {
                var transaction = await _context.ItemTransactions.Where(x => x.TarnsId == transactionId).FirstOrDefaultAsync();
                if (transaction==null)
                {
                    response.status = false;
                    response.message = "العملية غير موجودة";
                    return response;
                }
                _context.ItemTransactions.Remove(transaction);
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم مسح العملية بنجاح";
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<ItemTransactionResponse>> EditTransaction(ItemTransactionResponse input)
        {
            var response = new Response<ItemTransactionResponse>();
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<ItemTransactionResponse>> GetAllEmployeeTransactions(int empId)
        {
            var response = new Response<ItemTransactionResponse>();
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<ItemResponse>> GetEmployeeItems(int empId)
        {
            var response = new Response<ItemResponse>();
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<EmployeeResponse>> GetProductivityEmployees()
        {
            var response = new Response<EmployeeResponse>();
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<ItemTransactionResponse>> GetTransactionsByDate(ProductivitySalaryInput input)
        {
            var response = new Response<ItemTransactionResponse>();
            try
            {
                return null;
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
