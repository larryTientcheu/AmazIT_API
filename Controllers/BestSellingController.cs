using AmazIT_API.DatabaseClasses;
using AmazIT_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleRESTAPI.Models;


namespace AmazIT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestSellingController : ControllerBase
    {

        OrderItemDbManager db = new OrderItemDbManager();
        [HttpGet(Name = "GetBestSellingProducts")]
        public ActionResult<BestSelling> Get()
        {
            var bestSelling = db.GetBestSelling();
            if (bestSelling == null)
            {
                return NotFound();
            }
            return bestSelling;
            
        }
    }
}
