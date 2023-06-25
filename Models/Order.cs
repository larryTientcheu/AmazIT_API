using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SampleRESTAPI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public required Customer Customer { get; set; }

        public string? OrderDate { get; set; }

        public float Total { get; set; }

        public required ICollection<OrderItem> OrderItems { get; set; }

    }
}
