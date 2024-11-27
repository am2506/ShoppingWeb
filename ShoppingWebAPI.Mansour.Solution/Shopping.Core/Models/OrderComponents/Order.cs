using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Models.OrderComponents
{
    public class Order : BaseEntity
    {
        //To Ensure that user get their Order only and may user userId
        public string BuyerEmail { get; set; } = null!;

        public DateTimeOffset OrderTime { get; set; } = DateTimeOffset.UtcNow; // UTC time
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Processing;
        public Address ShippingAddress { get; set; } = null!;
        public decimal SubTotal { get; set; }
        [NotMapped]
        public decimal Total { get => SubTotal + DeliveryMethod?.Cost??0m;}

        public string PaymentIntentId { get; set; }

        //Navigation Property[One]
        public DeliveryMethod? DeliveryMethod { get; set; } 
        public int DeliveryMethodId { get; set; } //Foreign Key
        //Navigation Property
        public ICollection<OrderItem>OrderItems { get; set; } = null!;

        //private void CalculateSubTotal()
        //{
        //    // Sum all product prices
        //    SubTotal = OrderItems.Sum(Item => Item.Price);
        //}


    }
}
