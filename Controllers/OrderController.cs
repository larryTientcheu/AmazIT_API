using AmazIT_API.DatabaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleRESTAPI.Models;


namespace SampleRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        OrderDbManager db = new OrderDbManager();

        [HttpGet(Name = "GetOrders")]
        public IEnumerable<Order> Get() {
            return db.GetOrders();
        }
    }
}
