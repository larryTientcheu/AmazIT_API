using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SampleRESTAPI.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        public string? Name { get; set; }

        public string? Category { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }
        
    }
}
