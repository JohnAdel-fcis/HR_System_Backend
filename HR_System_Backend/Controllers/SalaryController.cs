using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HR_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly HR_DBContext _context;

        public SalaryController(HR_DBContext context)
        {
            _context = context;
        }




        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response<SalaryTypeResponse>();
            try
            {
                var typeList = await _context.SalaryTypes.Select(x => new SalaryTypeResponse { Name = x.SalaryTypeName, salaryTypeId = x.SalaryTypeId }).ToListAsync();
                if (typeList.Count > 0)
                {
                    response.status = true;
                    response.message = "تم استرجاع البيانات بنجاح";
                    response.data = typeList;
                    return Ok(response);
                }
                else
                {
                    response.status = false;
                    response.message = "ليس هناك اي بيانات";
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = new Response<SalaryTypeResponse>();
            try
            {
                var typeList = await _context.SalaryTypes.Where(x => x.SalaryTypeId == id).Select(x => new SalaryTypeResponse { Name = x.SalaryTypeName, salaryTypeId = x.SalaryTypeId }).ToListAsync();
                if (typeList.Count > 0)
                {
                    response.status = true;
                    response.message = "تم استرجاع البيانات بنجاح";
                    response.data = typeList;
                    return Ok(response);
                }
                else
                {
                    response.status = true;
                    response.message = "ليس هناك اي بيانات";
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] SalaryTypeInput input)
        {
            var response = new Response<SalaryTypeResponse>();
            try
            {
                var salaryType = new SalaryType
                {
                    SalaryTypeName = input.salaryTypeName
                };
                await _context.SalaryTypes.AddAsync(salaryType);
                await _context.SaveChangesAsync();



                response.status = true;
                response.message = "تم اضافة نوع الراتب بنجاح";
                response.data.Add(new SalaryTypeResponse
                {
                    Name = salaryType.SalaryTypeName,
                    salaryTypeId = salaryType.SalaryTypeId
                });


                return Ok(response);

            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return NotFound(response);
            }

        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] SalaryTypeResponse input)
        {
            var response = new Response<SalaryTypeResponse>();
            try
            {

                var salaryType = _context.SalaryTypes.Where(x => x.SalaryTypeId == input.salaryTypeId).FirstOrDefault();
                if (salaryType == null)
                {
                    response.status = false;
                    response.message = "نوع الراتب غير موجود";
                    return NotFound(response);

                }
                salaryType.SalaryTypeName = input.Name;
                await _context.SaveChangesAsync();
                response.status = true;
                response.message = "تم تعديل نوع الراتب بنجاح";
                response.data.Add(input);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return NotFound(response);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new Response<SalaryTypeResponse>();
            try
            {
                var type = await _context.SalaryTypes.FindAsync(id);
                if (type ==null)
                {
                    response.status = false;
                    response.message = "لا يوجد نوع راتب ";
                    return NotFound(response);
                }
                _context.SalaryTypes.Remove(type);
                await _context.SaveChangesAsync();
                var typeResp = new SalaryTypeResponse { Name = type.SalaryTypeName, salaryTypeId = type.SalaryTypeId };
                response.status = true;
                response.message = "تم حذف نوع الراتب بنجاح ";
                response.data.Add(typeResp);
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return NotFound(response);
            }
        }
    
     

    }
}