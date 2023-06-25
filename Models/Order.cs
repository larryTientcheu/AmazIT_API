using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SampleRESTAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public string? OrderDate { get; set; }

        public double Total { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }

    }
}
