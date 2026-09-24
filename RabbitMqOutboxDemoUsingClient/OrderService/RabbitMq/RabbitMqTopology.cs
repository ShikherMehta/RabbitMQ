using System.Threading.Channels;
using RabbitMQ.Client;

namespace OrderService.RabbitMq
{
    public class RabbitMqTopology
    {
        public const string OrderExchange = "order.exchange";

        public const string PaymentQueue = "payment.queue";

        public const string PaymentExchange = "payment.exchange";

        public const string PaymentDlExchange = "payment.dlx";

        public const string PaymentDlq = "payment.dlq";

        public const string OrderCreatedRoutingKey = "order.created";

        public const string PaymentFailedRoutingKey = "payment.failed";

        public static async Task ConfigureAsync(
            IChannel channel)
        {
            // Main exchange

            await channel.ExchangeDeclareAsync(
                OrderExchange,
                ExchangeType.Direct,
                durable: true);

            // Payment exchange

            await channel.ExchangeDeclareAsync(
                PaymentExchange,
                ExchangeType.Direct,
                durable: true);

            // DLX

            await channel.ExchangeDeclareAsync(
                PaymentDlExchange,
                ExchangeType.Direct,
                durable: true);

            // Payment queue

            var queueArguments =
                new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] =
                        PaymentDlExchange,

                    ["x-dead-letter-routing-key"] =
                        PaymentFailedRoutingKey
                };

            await channel.QueueDeclareAsync(
                PaymentQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: queueArguments);

            // Bind payment queue

            await channel.QueueBindAsync(
                PaymentQueue,
                PaymentExchange,
                OrderCreatedRoutingKey);

            // DLQ

            await channel.QueueDeclareAsync(
                PaymentDlq,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind DLQ to DLX

            await channel.QueueBindAsync(
                PaymentDlq,
                PaymentDlExchange,
                PaymentFailedRoutingKey);
        }
    }
}
