using AmazIT_API.DatabaseClasses;
using AmazIT_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleRESTAPI.Models;

namespace SampleRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrdersController : ControllerBase
    {
        ProductDbManager db =new ProductDbManager();

        [HttpGet(Name = "GetDetailedCustomerOrders")]
        public ActionResult<List<CustomerOrders>> Get([FromQuery] int customer_id)
        {
            var detailed_orders = db.GetProductsByCustomer(customer_id);
            if (detailed_orders == null)
                return NotFound();
            return detailed_orders;

        }
    }
}
