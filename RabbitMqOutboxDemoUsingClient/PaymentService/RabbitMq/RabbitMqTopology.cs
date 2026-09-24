using RabbitMQ.Client;

namespace PaymentService.RabbitMq
{
    public static class RabbitMqTopology
    {
        // Main Payment Exchange
        public const string PaymentExchange =
            "payment.exchange";

        public const string OrderCreatedRoutingKey =
            "order.created";

        // Main Payment Queue
        public const string PaymentQueue =
            "payment.queue";

        // Retry Exchange
        public const string PaymentRetryExchange =
            "payment.retry.exchange";

        // Retry Queue
        public const string PaymentRetryQueue =
            "payment.retry.queue";

        // Retry Routing Key
        public const string PaymentRetryRoutingKey =
            "payment.retry";

        // Dead Letter Exchange
        public const string PaymentDlxExchange =
            "payment.dlx";

        // Dead Letter Queue
        public const string PaymentDlq =
            "payment.dlq";

        // Failed routing key
        public const string PaymentFailedRoutingKey =
            "payment.failed";


        public static async Task ConfigureAsync(
            IChannel channel)
        {
            // =========================================
            // 1. MAIN PAYMENT EXCHANGE
            // =========================================

            await channel.ExchangeDeclareAsync(
                PaymentExchange,
                ExchangeType.Direct,
                durable: true);


            // =========================================
            // 2. MAIN PAYMENT QUEUE
            // =========================================

            var paymentQueueArguments =
                new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] =
                        PaymentDlxExchange,

                    ["x-dead-letter-routing-key"] =
                        PaymentFailedRoutingKey
                };

            await channel.QueueDeclareAsync(
                PaymentQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: paymentQueueArguments);


            // =========================================
            // 3. MAIN QUEUE BINDING
            // =========================================

            await channel.QueueBindAsync(
                PaymentQueue,
                PaymentExchange,
                OrderCreatedRoutingKey);


            // =========================================
            // 4. RETRY EXCHANGE
            // =========================================

            await channel.ExchangeDeclareAsync(
                PaymentRetryExchange,
                ExchangeType.Direct,
                durable: true);


            // =========================================
            // 5. RETRY QUEUE
            // =========================================

            var retryArguments =
                new Dictionary<string, object?>
                {
                    // Wait 5 seconds
                    ["x-message-ttl"] = 5000,

                    // After TTL, send message back
                    // to the main Payment Exchange
                    ["x-dead-letter-exchange"] =
                        PaymentExchange,

                    ["x-dead-letter-routing-key"] =
                        OrderCreatedRoutingKey
                };

            await channel.QueueDeclareAsync(
                PaymentRetryQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: retryArguments);


            // =========================================
            // 6. RETRY QUEUE BINDING
            // =========================================

            await channel.QueueBindAsync(
                PaymentRetryQueue,
                PaymentRetryExchange,
                PaymentRetryRoutingKey);


            // =========================================
            // 7. DEAD LETTER EXCHANGE
            // =========================================

            await channel.ExchangeDeclareAsync(
                PaymentDlxExchange,
                ExchangeType.Direct,
                durable: true);


            // =========================================
            // 8. DEAD LETTER QUEUE
            // =========================================

            await channel.QueueDeclareAsync(
                PaymentDlq,
                durable: true,
                exclusive: false,
                autoDelete: false);


            // =========================================
            // 9. DLQ BINDING
            // =========================================

            await channel.QueueBindAsync(
                PaymentDlq,
                PaymentDlxExchange,
                PaymentFailedRoutingKey);
        }
    }
}

