DeliveryTag in case of ack.
ACK tells RabbitMQ that the consumer has successfully processed the delivery. deliveryTag identifies the delivery, 
and multiple:false acknowledges only that specific delivery.


Very important

If you simply don't ACK and don't NACK, the message remains unacknowledged while that consumer/channel is alive.

If the consumer/channel/connection dies, RabbitMQ can requeue the unacknowledged message for another delivery.

For production systems, you normally combine this with:

ACK → Retry → NACK → DLQ

rather than endlessly requeueing a message that will always fail.
