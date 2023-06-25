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

        [HttpPost(Name ="CreateOrder")]
        public IActionResult Create([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                int newOrderId = db.AddOrder(order);
                order.OrderId = newOrderId;
                return CreatedAtRoute("GetOrders", new {id = newOrderId}, order);
            }
            else
                return BadRequest();
        }
        
        [HttpPatch("{id}", Name ="UpdateOrder")]
        public IActionResult Put(int id, [FromBody] Order order)
        {
             if (order == null)
                 return BadRequest();
                    
             var existingOrder = db.GetOrder(id);
                if (existingOrder == null)              
                    return NotFound();            

             existingOrder.CustomerId = order.CustomerId;
             existingOrder.OrderDate = order.OrderDate;
             existingOrder.Total = order.Total;

             db.UpdateOrder(existingOrder);
             return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteOrder(id))           
                return NoContent();          
            else            
                return NotFound();            
        }


    }
}
