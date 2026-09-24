namespace OrderService.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; } = "";

        public decimal Amount { get; set; }

        public string Status { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
