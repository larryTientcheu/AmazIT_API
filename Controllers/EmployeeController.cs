using AmazIT_API.DatabaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleRESTAPI.DatabaseClasses;
using SampleRESTAPI.Models;

namespace SampleRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        EmployeeDbManager db = new EmployeeDbManager();

        [HttpGet(Name = "GetEmployees")]
        public IEnumerable<Employee> Get()
        {
            return db.GetEmployees();
        }

        [HttpGet("{id}", Name = "GetEmployee")]
        public ActionResult<Employee> Get(int id)
        {
            var employee = db.GetEmployee(id);
            if (employee == null)
                return NotFound();
            return employee;
        }

        [HttpPost(Name ="CreateEmployees")]
        public IActionResult Post([FromBody] Employee employee) 
        {
            if (ModelState.IsValid)
            {
                int newEmployeeId = db.AddEmployee(employee);
                employee.EmployeeId = newEmployeeId;
                return CreatedAtRoute("GetEmployees", new { id = newEmployeeId }, employee);
            }
            else
                return BadRequest();
        }

        [HttpPatch("{id}", Name = "UpdateEmployee")]
        public IActionResult Patch(int id, [FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest();

            var existingEmployee = db.GetEmployee(id);
            if (existingEmployee == null)
                return NotFound();

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone= employee.Phone;
            existingEmployee.HireDate= employee.HireDate;
            existingEmployee.Salary= employee.Salary;
            existingEmployee.IsManager= employee.IsManager;

            db.UpdateEmployee(existingEmployee);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteEmployee")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteEmployee(id))
                return NoContent();
            else
                return NotFound();
        }


    }
}
