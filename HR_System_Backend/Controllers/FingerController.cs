
using System;
using System.Linq;
using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using HR_System_Backend.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HR_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FingerController : ControllerBase
    {
        private readonly IFingerRepository Irepo;

        public FingerController(IFingerRepository repo)
        {
            Irepo = repo;
        }


        [HttpPost]
        [Route("GetAll")]
        public async  Task<IActionResult> GetAll(FingerGetAllInput input )
        {
            try
            {
                var response = await Irepo.GetLogsFromDevice(input);
                if (response.status)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }

            }
            catch (Exception )
            {

                throw;
            }


        }

        [HttpPost]
        [Route("SetUser")]
        public async Task<IActionResult> SetUser(FingerGetAllInput input , string name , string password , int userId)
        {
            try
            {
                var response =  Irepo.SetUserFinger(userId, name, input, password);
                if (response.status)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }

            }
            catch (Exception)
            {

                throw;
            }


        }


        [HttpPost]
        [Route("GetUsersInfo")]
        public async Task<IActionResult> GetUsersInfo(FingerGetAllInput input)
        {
            try
            {
                var response = await Irepo.GetUsersInfoFromDevice(input);
                if (response.status)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }

            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}