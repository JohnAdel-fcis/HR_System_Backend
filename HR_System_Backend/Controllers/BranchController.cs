using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using HR_System_Backend.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchRepository Irepo;
        public BranchController(IBranchRepository repo)
        {
            Irepo = repo;
        }

        [HttpPost]
        [Route("AddBranch")]
        public async Task<IActionResult> AddBranch( string branchName)
        {
            try
            {
                var response = await Irepo.AddBranch(branchName);
                if (!response.status)
                {
                    return NotFound(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("EditBranch")]
        public async Task<IActionResult> EditBranch(BranchResponse input)
        {
            try
            {
                var response = await Irepo.EditBranch(input);
                if (!response.status)
                {
                    return NotFound(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [Route("DeleteBranch")]
        public async Task<IActionResult> DeleteBranch(BranchResponse input)
        {
            try
            {
                var response = await Irepo.DeleteBranch(input);
                if (!response.status)
                {
                    return NotFound(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var response = await Irepo.GetAllBranches();
                if (!response.status)
                {
                    return NotFound(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("GetEmployeesByBranch/{id}")]
        public async Task<IActionResult> GetEmployeesByBranch(int id)
        {
            try
            {
                var response = await Irepo.GetAllEmployeesInBranch(id);
                if (!response.status)
                {
                    return NotFound(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("GetDevicesByBranch/{id}")]
        public async Task<IActionResult> GetDevicesByBranch(int id)
        {
            try
            {
                var response = await Irepo.GetAllDevicesInBranch(id);
                if (!response.status)
                {
                    return NotFound(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        [HttpGet]
        [Route("GetBranchById/{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            try
            {
                var response = await Irepo.GetBranch(id);
                if (!response.status)
                {
                    return NotFound(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}