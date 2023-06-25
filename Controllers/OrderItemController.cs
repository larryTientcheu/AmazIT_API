using AmazIT_API.DatabaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleRESTAPI.Models;

namespace SampleRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        OrderItemDbManager db = new OrderItemDbManager();

        [HttpGet(Name ="GetOrderItems")]
        public IEnumerable<OrderItem> Get()
        {
            return db.GetOrderItems();
        }

        [HttpGet("{id}", Name = "GetOrderItem")]
        public ActionResult<OrderItem> Get(int id)
        {
            var orderItem = db.GetOrderItem(id);
            if (orderItem == null)
                return NotFound();
            return orderItem;
        }

        [HttpPost(Name = "CreateOrderItem")]
        public IActionResult Post([FromBody] OrderItem orderItem)
        {
            if(ModelState.IsValid)
            {
                int newOrderItem = db.AddOrderItem(orderItem);
                orderItem.OrderId = newOrderItem;
                return CreatedAtRoute("GetOrders", new { id = newOrderItem }, orderItem);
            }
            else
                return BadRequest();
        }

        [HttpPatch("{id}", Name ="UpdateOrderItem")]
        public IActionResult Patch(int id, [FromBody] OrderItem orderItem)
        {
            if(orderItem == null) 
                return BadRequest();

            var existingOrderItem = db.GetOrderItem(id);
            if (existingOrderItem == null)
                return NotFound();

            existingOrderItem.OrderId = orderItem.OrderId;
            existingOrderItem.ProductId = orderItem.ProductId;
            existingOrderItem.Quantity = orderItem.Quantity;
            existingOrderItem.Price = orderItem.Price;

            db.UpdateOrderItem(existingOrderItem);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrderItem")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteOrderItem(id))
                return NoContent();
            else
                return NotFound();
        }

    }
}
