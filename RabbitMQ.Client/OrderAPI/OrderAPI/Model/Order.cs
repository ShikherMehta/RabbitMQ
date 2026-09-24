namespace OrderAPI.Model
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string Status { get; set; } = "Created";

        public DateTime CreatedAt { get; set; }
    }
}
