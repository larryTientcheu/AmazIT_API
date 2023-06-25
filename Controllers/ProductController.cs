using AmazIT_API.DatabaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleRESTAPI.DatabaseClasses;
using SampleRESTAPI.Models;

namespace SampleRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ProductDbManager db = new ProductDbManager();


        [HttpGet(Name = "GetAllProducts")]
        public ActionResult<List<Product>> Get([FromQuery] string name="", string category="")
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(category))
                return db.GetProducts();

            else if (!string.IsNullOrEmpty(name))
            {
                var product = db.GetProductsByName(name);
                if (product == null)
                    return NotFound();
                return product;
            }
            else if (!string.IsNullOrEmpty(category))
            {
                var product = db.GetProductsByCategory(category);
                if (product == null)
                    return NotFound();
                return product;
            }
            else { return NotFound(); }

        }
        /*[HttpGet("getbyname", Name = "GetProductsByName")]
        public ActionResult<List<Product>> GetProductByName([FromQuery] string name)
        {
            var product = db.GetProductsByName(name);
            if (product == null)
                return NotFound();
            return product;
        }*/

        [HttpGet("{id}", Name = "GetProduct")]
        public ActionResult<Product> Get(int id)
        {
            var product = db.GetProductById(id);
            if (product == null)
                return NotFound();          
            return product;
        }
       
        
        

        [HttpPost(Name = "CreateProduct")]
        public IActionResult Create([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                int newProductId = db.AddProduct(product);
                product.ProductID = newProductId;
                return CreatedAtRoute("GetProduct", new { id = newProductId }, product);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if (product == null || product.ProductID != id)
            {
                return BadRequest();
            }

            Product? existingProduct = db.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Category = product.Category;

            db.UpdateProduct(existingProduct);

            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteProduct")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteProduct(id))
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
