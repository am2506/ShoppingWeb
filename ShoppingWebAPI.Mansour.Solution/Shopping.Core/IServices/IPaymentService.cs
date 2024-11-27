using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models.Basket;
using Shopping.Core.Models.OrderComponents;

namespace Shopping.Core.IServices
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId); // Return basket with payment Intent
        Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid);
        
    }
}
