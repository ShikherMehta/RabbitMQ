DeliveryTag in case of ack.
ACK tells RabbitMQ that the consumer has successfully processed the delivery. deliveryTag identifies the delivery, 
and multiple:false acknowledges only that specific delivery.


Very important

If you simply don't ACK and don't NACK, the message remains unacknowledged while that consumer/channel is alive.

If the consumer/channel/connection dies, RabbitMQ can requeue the unacknowledged message for another delivery.

For production systems, you normally combine this with:

ACK → Retry → NACK → DLQ

rather than endlessly requeueing a message that will always fail.


Key interview point:

requeue:false does not mean "requeue somewhere else." It means RabbitMQ will not return the message to the original queue. With a DLX configured, the message can be dead-lettered; otherwise it is discarded.
Why is it called a delivery?

Because RabbitMQ doesn't just think:

"Message B exists."

It thinks:

"I delivered this particular message to this consumer on this channel."

The deliveryTag identifies that delivery.
