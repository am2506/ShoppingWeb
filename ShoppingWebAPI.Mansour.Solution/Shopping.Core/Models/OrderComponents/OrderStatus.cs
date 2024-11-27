using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Models.OrderComponents
{
    public enum OrderStatus
    {
        [EnumMember]
        Pending,        // The order has been placed but not processed yet.
        [EnumMember]
        Processing,     // The order is being prepared or packed.
        [EnumMember]
        Shipped,        // The order has been shipped and is on the way.
        [EnumMember]
        Delivered,      // The order has been delivered to the customer.
        [EnumMember]
        Canceled,       // The order has been canceled.
        [EnumMember]
        Returned,       // The order has been returned by the customer.
        [EnumMember]
        Refunded,       // The payment has been refunded for the order.
        [EnumMember]
        Failed          // The order has failed due to payment or processing issues.
    }

}
