using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HR_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftRepository Irepo;

        public ShiftController(IShiftRepository repo)
        {
            Irepo = repo;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await Irepo.GetAllShifts();
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }

        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await Irepo.GetShift(id);
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }

        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] ShiftInput input)
        {
            var response = await Irepo.AddShift(input);
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }


        [HttpPost]
        [Route("AddOvertime")]
        public async Task<IActionResult> AddOvertime([FromBody] OverTimeInput input)
        {
            var response = await Irepo.AddOverTime(input);
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] ShiftResponse input)
        {
            var response = await Irepo.EditShift(input);
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }


        [HttpPost]
        [Route("AddExcuseAbsence")]
        public async Task<IActionResult> AddExcuseAbsence([FromBody] AbsenceInput input)
        {
            var response = await Irepo.AddExcuseAbsence(input);
            if (response.status)
            {
                return Ok(response);
                
            }
            else
            {
                return NotFound(response);
            }
        }


        [HttpPost]
        [Route("GetAbsence")]
        public async Task<IActionResult> GetAbsence([FromBody] AttendLeaveReportInput input)
        {
            var response = await Irepo.GetAllAbsenceDays(input);
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await Irepo.DeleteShift(id);
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