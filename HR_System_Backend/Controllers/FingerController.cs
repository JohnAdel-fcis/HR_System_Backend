
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Helper;
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


        [HttpGet]
        [Route("GetDevices")]
        public async Task<IActionResult> GetDevices()
        {
            try
            {
                var response = await Irepo.GetAllDevices();
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


        [HttpGet]
        [Route("GetDevice/{id}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            try
            {
                var response = await Irepo.GetDeviceByid(id);
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
        [Route("GetAllLogs")]
        public async  Task<IActionResult> GetAllLogs(FingerGetAllInput input )
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
                var response =   Irepo.SetUserFinger(userId, name,1, input, password);
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



        [HttpPost]
        [Route("SaveUsersDb")]
        public async Task<IActionResult> SaveUsersDb(List<GetUserInfoResponse> input)
        {
            try
            {
                var response = await Irepo.SaveUsersInfoToDb(input);
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
        [Route("SaveLogsDb")]
        public async Task<IActionResult> SaveLogsDb(List<EmpInfoFinger> input)
        {
            try
            {
                var response = await Irepo.SaveLogsToDb(input);
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
        [Route("AddDevice")]
        public async Task<IActionResult> AddDevice(DeviceInput input)
        {
            try
            {
                var response = await Irepo.AddDevice(input);
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
        [Route("EditDevice")]
        public async Task<IActionResult> EditDevice(DeviceResponse input)
        {
            try
            {
                var response = await Irepo.EditDevice(input);
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