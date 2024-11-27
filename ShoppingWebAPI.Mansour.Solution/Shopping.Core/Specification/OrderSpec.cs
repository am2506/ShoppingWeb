using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models.OrderComponents;
using Shopping.Repository.SpecificationDesignPattern;

namespace Shopping.Core.Specification
{
    public class OrderSpec : BaseSpecification<Order>
    {
        public OrderSpec(string buyerEmail):base(order => order.BuyerEmail == buyerEmail)
        {
            AddIncludeExpression(order => order.DeliveryMethod);
            AddIncludeExpression(order => order.OrderItems);
        }
        public OrderSpec(string buyerEmail, int orderId):
            base(order => order.BuyerEmail == buyerEmail && order.Id == orderId)
        {
            AddIncludeExpression(order => order.DeliveryMethod);
            AddIncludeExpression(order => order.OrderItems);
        }
        public OrderSpec(string paymentIntentId, bool isPaymentIntent) :base(order => order.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
