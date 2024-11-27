using System.ComponentModel.DataAnnotations.Schema;
using Shopping.Core.Models.OrderComponents;

namespace RouteWebAPI.Dtos.OrderDto
{
    public class OrderResponseDto
    {
        public DateTimeOffset OrderTime { get; set; } 
        public string OrderStatus { get; set; } 
        public Address ShippingAddress { get; set; } = null!;
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public decimal Total { get; set; }

        //Navigation Property
        public ICollection<OrderItemDto> OrderItems { get; set; } = null!;

        public string? PaymentIntentId { get; set; }
    }
}
