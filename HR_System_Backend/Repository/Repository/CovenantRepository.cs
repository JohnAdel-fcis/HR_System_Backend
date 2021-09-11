
using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Repository.Repository
{
    public class CovenantRepository : ICovenantRepository
    {
        private readonly HR_DBContext _context;
        public CovenantRepository( HR_DBContext context)
        {
            _context = context;
        }




        public async Task<Response<bool>> AddCovenant(CovenantInput input)
        {
            var response = new Response<bool>();
            try
            {
                var employee = _context.Employees.Where(x => x.Id == input.EmployeeId).FirstOrDefault();
                if (employee == null)
                {
                    response.status = false;
                    response.message = "الموظف غير موجود";
                    return response;
                }
                employee.Covenants.Add(new Covenant
                {
                    EmplyeeId = input.EmployeeId,
                    CovenantName= input.CovenantName ,
                    CovenantFromDate =input.CovenantFromDate ,
                    CovenantToDate = input.CovenantToDate
                });
                await _context.SaveChangesAsync();

                response.status = true;
                response.message = "تم تسجيل العهدة بنجاح ";
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "حصل خطأ" + ex.Message;
                return response;
            }
        }

        public Task<Response<CovenantResponse>> DeleteCovenant(int coventantId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<CovenantResponse>> EditCovenant(CovenantResponse input)
        {
            throw new NotImplementedException();
        }

        public Task<Response<CovenantResponse>> GetAllCovenant()
        {
            throw new NotImplementedException();
        }

        public Task<Response<CovenantResponse>> GetCovenantById(int covenantId)
        {
            throw new NotImplementedException();
        }
    }
}
