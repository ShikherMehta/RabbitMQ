using RabbitMQ.Client;

namespace PaymentService.RabbitMq
{
    public class RabbitMqConnection
    {
        private readonly ConnectionFactory _factory;

        private IConnection? _connection;

        public RabbitMqConnection(
            IConfiguration configuration)
        {
            _factory = new ConnectionFactory
            {
                HostName =
                    configuration["RabbitMQ:Host"] ?? "localhost",

                Port =
                    int.Parse(
                        configuration["RabbitMQ:Port"] ?? "5672"),

                UserName =
                    configuration["RabbitMQ:Username"] ?? "admin",

                Password =
                    configuration["RabbitMQ:Password"] ?? "admin123"
            };
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            if (_connection is null ||
                !_connection.IsOpen)
            {
                _connection =
                    await _factory.CreateConnectionAsync();
            }

            return _connection;
        }
    }
}
