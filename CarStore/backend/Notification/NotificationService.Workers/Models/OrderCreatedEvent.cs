using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaConsumer
{
    internal class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }

        public string? BuyerEmail { get; set; }

        public string? OwnerEmail { get; set; }

        public string? ProductName { get; set; }
    }
}
