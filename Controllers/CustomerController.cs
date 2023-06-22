using AmazIT_API.DatabaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleRESTAPI.DatabaseClasses;
using SampleRESTAPI.Models;

namespace SampleRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        CustomerDbManager db = new CustomerDbManager("Data Source=DatabaseFile/AmazIT_API.db");


        [HttpGet(Name = "GetAllCustomers")]
        public IEnumerable<Customer> Get()
        {
            return db.GetCustomers();
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public ActionResult<Customer> Get(int id)
        {
            var customer = db.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        [HttpPost(Name = "CreateCustomer")]
        public IActionResult Create([FromBody] Customer customer)
        {
            if (ModelState.IsValid)
            {
                int newCustomerId = db.AddCustomer(customer);
                customer.CustomerID = newCustomerId;
                return CreatedAtRoute("GetCustomer", new { id = newCustomerId }, customer);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateCustomer")]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            if (customer == null || customer.CustomerID != id)
            {
                return BadRequest();
            }

            var existingCustomer = db.GetCustomerById(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;

            db.UpdateCustomer(existingCustomer);

            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteCustomer")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteCustomer(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
