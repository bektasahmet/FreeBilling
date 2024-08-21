using FreeBilling.Data.Entities;
using FreeBilling.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreeBilling.Web.Controllers
{
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IBillingRepository _repository;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ILogger<CustomersController> logger ,IBillingRepository repository)
        {
            _repository = repository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("api/customers")]
        public async Task<ActionResult<IEnumerable<Customer>>> Get(bool withAddresses = false)
        {
            try
            {
                IEnumerable<Customer> results; 
                
                if (withAddresses)
                {
                    results = await _repository.GetCustomersWithAddresses();
                }
                else
                {
                    results = await _repository.GetCustomers();
                }

                return Ok(results);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to get customers from database.");
                return Problem("Failed to get customers from database.");
            }
            
        }

        [HttpGet("api/customers/{id:int}")]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            try
            {
                var result = await _repository.GetCustomerById(id);

                if (result is null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown while reading customer");
                return Problem($"Exception thrown: {ex:Message}");
            }
        }

        [HttpGet("api/customers/{name}")]
        public async Task<Customer?> GetByName(string name)
        {
            return await _repository.GetCustomerByName(name);
        }
    }
}
