using System.Threading.Tasks;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;

namespace HR_System_Backend.Repository.Interface
{
    public interface IReportRepository
    {
         Task<Response<AttendLeaveReportResponse>> AttendAndLeaveReport(AttendLeaveReportInput input);
        Task<Response<ProductivitySalaryReportResponse>> ProductivitySalaryReport(ProductivitySalaryInput input);


    }
}