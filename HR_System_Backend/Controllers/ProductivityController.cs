
using Microsoft.AspNetCore.Mvc;
using HR_System_Backend.Repository.Interface;
using HR_System_Backend.Repository.Repository;
using System.Threading.Tasks;
using System;
using HR_System_Backend.Model.Input;
using HR_System_Backend.Model.Response;

namespace HR_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductivityController : ControllerBase
    {
        private readonly IProductiviyRepository Irepo;
        public ProductivityController(IProductiviyRepository repo)
        {
            Irepo = repo;
        }

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var response = await Irepo.GetProductivityEmployees();
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
        [Route("GetEmployeeItems/{id}")]
        public async Task<IActionResult> GetEmployeeItems(int id)
        {
            try
            {
                var response = await Irepo.GetEmployeeItems(id);
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
        [Route("GetTransactionsByDate")]
        public async Task<IActionResult> GetTransactionsByDate(ProductivitySalaryInput input)
        {
            try
            {
                var response = await Irepo.GetTransactionsByDate(input);
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
        [Route("GetEmployeeTransactionsByDate")]
        public async Task<IActionResult> GetEmployeeTransactionsByDate(ProductivitySalaryInput input )
        {
            try
            {
                var response = await Irepo.GetEmployeeTransactionsByDate(input);
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
        [Route("GetAllEmployeeTransactions/{id}")]
        public async Task<IActionResult> GetAllEmployeeTransactions(int id)
        {
            try
            {
                var response = await Irepo.GetAllEmployeeTransactions(id);
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
        [Route("AddTransaction")]
        public async Task<IActionResult> AddTransaction(ItemTransactionInput input)
        {
            try
            {
                var response = await Irepo.AddTransaction(input);
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
        [Route("EditTransaction")]
        public async Task<IActionResult> EditTransaction(ItemTransactionResponse input)
        {
            try
            {
                var response = await Irepo.EditTransaction(input);
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
        [Route("CalculateSalary")]
        public async Task<IActionResult> CalculateSalary(ProductivitySalaryInput input)
        {
            try
            {
                var response = await Irepo.CalculateSalary(input);
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
























        [HttpDelete]
        [Route("DeleteTransaction")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            try
            {
                var response = await Irepo.DeleteTransaction(transactionId);
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