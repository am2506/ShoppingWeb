using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models.OrderComponents;

namespace Shopping.Core.IServices
{
    public interface IOrderService
    {
        //Parameters 
        //1.buyer Email
        //2.basket Id - Get basket items
        //3. Address -- Get shipping Address
        //4. Payment Intent Id
        //5.Delivery Method Id
        Task<Order?> CreateOrderAsync(string buyerEmail,string basketId , Address address,int deliveryId);
        Task<ICollection<Order>> GetUserOrdersByEmailAsync(string email);
        Task<Order?> GetUserOrderByOrderIdAsync(string email, int OrderId);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}
