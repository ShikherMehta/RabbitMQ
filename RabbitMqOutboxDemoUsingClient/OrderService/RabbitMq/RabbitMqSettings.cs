namespace OrderService.RabbitMq
{
    public class RabbitMqSettings
    {
        public string Host { get; set; } = "localhost";

        public int Port { get; set; } = 5672;

        public string Username { get; set; } = "admin";

        public string Password { get; set; } = "admin123";
    }
}
