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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository Irepo;
        public EmployeeController(IEmployeeRepository repo)
        {
            Irepo = repo;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response<EmployeeResponse>();
            try
            {
                
                var result = await Irepo.GetAllEmployes();
                if (result.status)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);

                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return NotFound(response);
            }
        }
        
        [HttpGet]
        [Route("GetDetails")]
        public async Task<IActionResult> GetDetails()
        {
            var response = new Response<EmployeeResponse>();
            try
            {

                var result = await Irepo.GetAllDetailsEmployes();
                if (result.status)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);

                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return NotFound(response);
            }
        }

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {

            var response = new Response<RoleResponse>();
            try
            {

                var result = await Irepo.GetRoles();
                if (result.status)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);

                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return NotFound(response);
            }

        }


        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = new Response<EmployeeResponse>();
            try
            {  
                var result = await Irepo.GetEmployee(id);
                if (result.status)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);

                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return NotFound(response);
            }
        }

   
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] EmployeeInput input)
        {
            var result = await Irepo.AddEmployee(input);
            if (result.status)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] EmployeeResponse input)
        {
            var result = await Irepo.EditEmployee(input);
            if (result.status)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await Irepo.DeleteEmployee(id);
            if (result.status)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
    
            
    
    
    
    
    
    
    }
}