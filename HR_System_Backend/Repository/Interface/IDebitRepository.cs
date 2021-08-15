using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Interface
{
    public interface IDebitRepository
    {
        Task<Response<DebitResponse>> AddDebit(DebitInput input);
        Task<Response<DebitResponse>> PayDebit(DebitInput input);
        Task<Response<DebitResponse>> EditDebit(DebitResponse input);
        Task<Response<DebitResponse>> RemoveDebit(int id);
        Task<Response<DebitResponse>> GetAllDebits();
        Task<Response<DebitResponse>> GetDebit(int id);
        Task<Response<DebitResponse>> GetEmployeeDebits(int empId);
        Task<Response<DebitResponse>> PartPayment(PartPaymentInput payment);
        Task<Response<DebitTransactionResponse>> GetDebitTransactions(int debitID);


    }
}
