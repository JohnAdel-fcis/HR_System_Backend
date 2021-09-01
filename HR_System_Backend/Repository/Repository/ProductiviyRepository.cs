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

                foreach (var selectedTransaction in input.Transactions)
                {
                    if (selectedTransaction.Quantity == null || selectedTransaction.Quantity == 0)
                    {
                        continue;
                    }
                    var item = await _context.Items.Where(x => x.ItemId == selectedTransaction.ItemId).FirstOrDefaultAsync();
                    if (item == null)
                    {
                        response.status = false;
                        response.message = "نوع القطعه غير موجود ";
                        return response;
                    }

                    var commisionPerUnit = item.ItemPrice / item.ItemQnty;


                    var transaction = new ItemTransaction
                    {
                        ItemId = selectedTransaction.ItemId,
                        ItemQuantity = selectedTransaction.Quantity,
                        ItemComissions = (commisionPerUnit * selectedTransaction.Quantity),
                        TransDate = selectedTransaction.Date
                    };
                    employee.ItemTransactions.Add(transaction);
                    await _context.SaveChangesAsync();
                    response.data.Add(new ItemTransactionResponse
                    {
                        EmpId = input.EmployeeId,
                        ItemId = transaction.ItemId,
                        TarnsId = transaction.TarnsId,
                        ItemComissions = transaction.ItemComissions,
                        ItemQuantity = transaction.ItemQuantity,
                        TransDate = transaction.TransDate
                    });
                }
                response.status = true;
                response.message = "تم اضافة العمليه بنجاح";

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
                if (transaction == null)
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
                var transaction = await _context.ItemTransactions.Where(x=>x.TarnsId == input.TarnsId).FirstOrDefaultAsync();
                if(transaction==null){
                    response.status =false ;
                    response.message ="لا توجد هذه العملية";
                    return response ;
                }  
                transaction.EmpId=input.EmpId;
                transaction.ItemId = input.ItemId;
                transaction.ItemQuantity=input.ItemQuantity ;
                transaction.ItemComissions=input.ItemComissions;
                await _context.SaveChangesAsync();
                response.status =true ;
                response.message="تم التعديل بنجاح";
                return response ;
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
                var transactions = await _context.ItemTransactions.Select(x => new ItemTransactionResponse
                {
                    EmpId = x.EmpId,
                    ItemId = x.ItemId,
                    ItemQuantity = x.ItemQuantity,
                    ItemComissions = x.ItemComissions,
                    TransDate = x.TransDate,
                    TarnsId = x.TarnsId
                }).Where(x => x.EmpId == empId).ToListAsync();
                if (transactions.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد عمليات";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = transactions;
                return response;


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
                var employee = await _context.Employees.Include(x => x.Items).Where(x => x.Id == empId).FirstOrDefaultAsync();
                if (employee == null)
                {
                    response.status = false;
                    response.message = "الموظف غير موجود ";
                    return response;
                }
                var empItems = employee.Items.Select(x => new ItemResponse { ItemId = x.ItemId, ItemName = x.ItemName, ItemPrice = x.ItemPrice, ItemQnty = x.ItemQnty, ItemCommission = x.ItemQnty }).ToList();
                if (empItems.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد قطع لهذا الموظف";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = empItems;
                return response;
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
                var productivityEmployees = await _context.Employees.Select(x => new EmployeeResponse
                {
                    id = x.Id,
                    name = x.Name,
                    productivity = x.Productivity.Value
                }).Where(x => x.productivity == true).ToListAsync();
                if (productivityEmployees.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد موظفين في قسم الانتاجيه";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = productivityEmployees;
                return response;
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
                var transactions = await _context.ItemTransactions.Select(x => new ItemTransactionResponse
                {
                    EmpId = x.EmpId,
                    ItemId = x.ItemId,
                    ItemQuantity = x.ItemQuantity,
                    ItemComissions = x.ItemComissions,
                    TransDate = x.TransDate,
                    TarnsId = x.TarnsId
                }).Where(
                    x => x.TransDate.Value.Second >= input.From.Value.Second &&
                    x.TransDate.Value.Second <= input.To.Value.Second
                    ).ToListAsync();

                if (transactions.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد عمليات";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = transactions;
                return response;


            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<ItemTransactionResponse>> GetEmployeeTransactionsByDate(ProductivitySalaryInput input)
        {
            var response = new Response<ItemTransactionResponse>();
            try
            {
                var transactions = await _context.ItemTransactions.Select(x => new ItemTransactionResponse
                {
                    EmpId = x.EmpId,
                    ItemId = x.ItemId,
                    ItemQuantity = x.ItemQuantity,
                    ItemComissions = x.ItemComissions,
                    TransDate = x.TransDate,
                    TarnsId = x.TarnsId
                }).Where(
                    x => (x.EmpId == input.EmployeeId) && (x.TransDate.Value.Second >= input.From.Value.Second &&
                    x.TransDate.Value.Second <= input.To.Value.Second)
                    ).ToListAsync();

                if (transactions.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد عمليات";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = transactions;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }
            //rabee3 &abibi

        }
    }
}