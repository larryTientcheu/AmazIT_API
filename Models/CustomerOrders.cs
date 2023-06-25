namespace AmazIT_API.Models
{
    public class CustomerOrders
    {
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
    }
}
