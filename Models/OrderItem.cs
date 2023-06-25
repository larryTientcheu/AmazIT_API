using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SampleRESTAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public required Order Order { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public required Product Product { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
