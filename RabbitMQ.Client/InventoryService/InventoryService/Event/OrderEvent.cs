using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryService.Event
{
    public record OrderCreatedEvent(
       Guid OrderCreated,
       Guid CustomerId,
       Guid ProductId,
       int Quantity
   );
}
