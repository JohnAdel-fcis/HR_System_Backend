using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebitController : ControllerBase
    {
        private readonly IDebitRepository Irepo;
        public DebitController(IDebitRepository repo)
        {
            Irepo = repo;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await Irepo.GetAllDebits();
            if (result.status)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet]
        [Route("GetEmpDebit/{empId}")]
        public async Task<IActionResult> GetEmpDebit(int empId)
        {
            var result = await Irepo.GetEmployeeDebits(empId);
            if (result.status)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }


        [HttpGet]
        [Route("GetDebitTransactions/{debitId}")]
        public async Task<IActionResult> GetDebitTransactions(int debitId)
        {
            var result = await Irepo.GetDebitTransactions(debitId);
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
        public async Task<IActionResult> add([FromBody] DebitInput debit)
        {
            var result = await Irepo.AddDebit(debit);
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
        [Route("PartPayment")]
        public async Task<IActionResult> PartPayment(PartPaymentInput debit)
        {
            var result = await Irepo.PartPayment(debit);
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