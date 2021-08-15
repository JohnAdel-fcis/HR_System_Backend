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
    public class DebitRepository : IDebitRepository
    {
        private HR_DBContext _context;
        public DebitRepository(HR_DBContext context)
        {
            _context = context;
        }
        public async Task<Response<DebitResponse>> AddDebit(DebitInput input)
        {
            var response = new Response<DebitResponse>();
            try
            {
                var employee = await _context.Employees.Where(x => x.Id == input.EmployeeId).FirstOrDefaultAsync();
                if (employee == null)
                {
                    response.status = false;
                    response.message = "الموظف غير موجود";
                    return response;
                }
                var installmentDate = new DateTime();
                if (input.Installment != 0 && input.Installment != null)
                {
                    installmentDate = DateTime.Now.Date;
                }
                await _context.Debits.AddAsync(new Debit
                {
                    DebitName = input.DebitName,
                    DebitDate = input.DebitDate,
                    DebitAmount = input.DebitAmount,
                    EmployeeId = input.EmployeeId,
                    Installment = input.Installment,
                    LastInstallmentPayDate = installmentDate,
                    Finished = false,
                    PaidAmount = 0,
                    InstallmentPaidAmount = 0,
                    RemainingDebitAmount = input.DebitAmount,
                    Notes = input.Notes
                });
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تمت اضافه السلفة بنجاح";
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return response;
            }

        }

        public Task<Response<DebitResponse>> EditDebit(DebitResponse input)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<DebitResponse>> GetAllDebits()
        {
            var response = new Response<DebitResponse>();
            try
            {
                var debits = await _context.Debits.Select(x => new DebitResponse
                {
                    DebitAmount = x.DebitAmount,
                    DebitDate = x.DebitDate,
                    DebitName = x.DebitName,
                    PaidAmount = x.PaidAmount,
                    Installment = x.Installment,
                    EmployeeId = x.EmployeeId,
                    Notes = x.Notes,
                    DebitId = x.DebitId,
                    RemainingDebitAmount = x.RemainingDebitAmount,
                    Finished=x.Finished
                }).ToListAsync();

                if (debits.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد سلفات";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = debits;
                return response;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public Task<Response<DebitResponse>> GetDebit(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<DebitResponse>> GetEmployeeDebits(int empId)
        {
            var response = new Response<DebitResponse>();
            try
            {
                CheckDebitsInstallment();

                var debit = await _context.Debits.Where(x => x.EmployeeId == empId).Select(x => new DebitResponse
                {
                    DebitName = x.DebitName,
                    DebitAmount = x.DebitAmount,
                    DebitDate = x.DebitDate,
                    Installment = x.Installment,
                    PaidAmount = x.PaidAmount
                }).ToListAsync();

                if (debit.Count == 0)
                {
                    response.status = false;
                    response.message = "السلفه غير موجوده";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = debit;
                return response;





            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Response<DebitTransactionResponse>> GetDebitTransactions(int debitID)
        {
            var response = new Response<DebitTransactionResponse>();
            try
            {
                var debitTransactions = await _context.DebitTransactions.Where(x => x.DebitId == debitID).Select(x => new DebitTransactionResponse
                {
                    InstallmentPaidAmount = x.InstallmentPaidAmount,
                    DebitId = x.DebitId,
                    Instalment = x.Instalment,
                    IsInstalment = x.IsInstalment,
                    LastInstallmentPayDate = x.LastInstallmentPayDate,
                    Notes = x.Notes,
                    PaidAmount = x.PaidAmount,
                    PartialPayment = x.PartialPayment,
                    RemainingDebitAmount = x.RemainingDebitAmount,
                    TranDate = x.TranDate,
                    TranId = x.TranId
                }).ToListAsync();
                if (debitTransactions.Count == 0)
                {
                    response.status = false;
                    response.message = "لا يوجد اي عمليات";
                    return response;
                }
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = debitTransactions;
                return response;
            }
            catch (System.Exception ex)
            {
                response.status = false;
                response.message =ex.Message ;
                return response;
            }
        }
        public Task<Response<DebitResponse>> PayDebit(DebitInput input)
        {
            throw new NotImplementedException();
        }
        public async Task<Response<DebitResponse>> PartPayment(PartPaymentInput payment)
        {
            var response = new Response<DebitResponse>();
            try
            {
                var debit = await _context.Debits.Where(x => x.DebitId == payment.debitId).FirstOrDefaultAsync();
                if (debit == null)
                {
                    response.status = false;
                    response.message = "السلفة غير موجوده";
                    return response;
                }
                if (debit.RemainingDebitAmount < payment.paymentAmount)
                {
                    response.status = false;
                    response.message = "الملغ المتبقي اقل من مبلغ الدفعة ";
                    return response;
                }

                var remainingDebitAmount = debit.RemainingDebitAmount - payment.paymentAmount;
                var paidAmount = debit.PaidAmount + payment.paymentAmount;
                if (remainingDebitAmount == 0)
                {
                    debit.Finished = true;
                }
                else
                {
                    debit.Finished = false;
                }
                var transaction = new DebitTransaction
                {
                    IsInstalment = false,
                    PaidAmount = paidAmount,
                    RemainingDebitAmount = remainingDebitAmount,
                    PartialPayment = payment.paymentAmount,
                    TranDate = DateTime.Now

                };
                debit.PaidAmount = paidAmount;
                debit.RemainingDebitAmount = remainingDebitAmount;
                debit.DebitTransactions.Add(transaction);
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم انقاص الدفعة من السلفه بنجاح";
                response.data.Add(new DebitResponse
                {
                    DebitAmount = debit.DebitAmount,
                    DebitDate = debit.DebitDate,
                    EmployeeId = debit.EmployeeId,
                    PaidAmount = debit.PaidAmount,
                    Installment = debit.Installment,
                    DebitName = debit.DebitName,
                    Notes = debit.Notes,
                    RemainingDebitAmount = debit.RemainingDebitAmount,
                    Finished=debit.Finished
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
        public Task<Response<DebitResponse>> RemoveDebit(int id)
        {
            throw new NotImplementedException();
        }
        private void CheckDebitsInstallment()
        {
            try
            {

                var debits = _context.Debits.Where(x => x.Finished == false)/*.Where(s=>s.Installment != 0 ).Where(w=>w.Installment != null)*/.ToList();



                if (debits.Count == 0)
                {
                    return;
                }
                foreach (var debit in debits)
                {
                    if (DateTime.Now.Subtract(debit.LastInstallmentPayDate.Value).Seconds >= 1)
                    {
                        var Trans = new DebitTransaction();
                        var installment = debit.Installment.Value;
                        var remainingDebit = debit.RemainingDebitAmount.Value;
                        if (remainingDebit - installment < 0)
                        {
                            installment = remainingDebit;
                        }
                        Trans.IsInstalment = true;
                        Trans.Instalment = debit.Installment.Value;
                        Trans.TranDate = DateTime.Now;
                        Trans.PartialPayment = installment;

                        debit.PaidAmount += installment;
                        debit.RemainingDebitAmount -= installment;
                        debit.InstallmentPaidAmount += installment;
                        debit.LastInstallmentPayDate = DateTime.Now;

                        Trans.PaidAmount = debit.PaidAmount;
                        Trans.RemainingDebitAmount = debit.RemainingDebitAmount;
                        Trans.InstallmentPaidAmount = debit.InstallmentPaidAmount;
                        Trans.LastInstallmentPayDate = DateTime.Now;

                        debit.DebitTransactions.Add(Trans);
                        if (debit.PaidAmount == debit.DebitAmount)
                        {
                            debit.Finished = true;
                        }
                        _context.SaveChanges();





                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
