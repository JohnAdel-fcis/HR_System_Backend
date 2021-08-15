using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using HR_System_Backend.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository Irepo;
        public CategoryController(ICategoryRepository repo)
        {
            Irepo = repo;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response<CategoryResponse>();
            try
            {

                var result = await Irepo.GetAll();
                if (result.status)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            catch (Exception)
            {
                response.status = false;
                response.message = "حدث خظأ";
                return NotFound(response);
            }
        }


        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await Irepo.Get(id);
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
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]CategoryInput input)
        {
            var result = await Irepo.AddCategory(input);
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
        public async Task<IActionResult> Edit([FromBody]CategoryResponse input)
        {
            var result = await Irepo.EditCategory(input);
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
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Irepo.DeleteCategory(id);

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