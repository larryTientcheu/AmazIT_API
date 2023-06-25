using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SampleRESTAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public required Order? Order { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }        
        public required Product? Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
