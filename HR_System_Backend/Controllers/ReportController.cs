
using System.Threading.Tasks;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HR_System_Backend.Controllers
{
     [Route("api/[controller]")]
    [ApiController]

    public class ReportController :ControllerBase
    {
        private readonly IReportRepository  Irepo;
        public ReportController(IReportRepository repo)
        {
            Irepo = repo ;
        }

        [HttpPost]
        [Route("GetAttendReport")]
        public async Task<IActionResult> GetAttendReport( AttendLeaveReportInput input )
        {
           var response = await Irepo.AttendAndLeaveReport(input);
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }







    }
}