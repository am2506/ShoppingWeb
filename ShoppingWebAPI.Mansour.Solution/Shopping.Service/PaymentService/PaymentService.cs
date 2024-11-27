using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core;
using Shopping.Core.IRepository;
using Shopping.Core.IServices;
using Shopping.Core.Models.Basket;
using Shopping.Core.Models.OrderComponents;
using Shopping.Core.Specification;
using Product = Shopping.Core.Models.Product;
namespace Shopping.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepo,IUnitOfWork unitOfWork)
        {
           _configuration = configuration;
           _basketRepo = basketRepo;
           _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Strip:Secretkey"];
            //To Create Payment Intent with options 
            //1. Calculate amount
            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket is null)
                return null;
            var shippingCost = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod is not null)
                {
                    shippingCost = deliveryMethod.Cost;
                    basket.ShippingPrice = shippingCost; 
                }

            }

            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                    if (product is not null)
                    {
                        if (item.Price != product.Price)
                            item.Price = product.Price;
                    }
                }
            }

            PaymentIntent paymentIntent;

            //To Create Payment Intent
            PaymentIntentService paymentIntentService = new PaymentIntentService(); 

            if(string.IsNullOrEmpty(basket?.paymentIntendId)) //Create New Payment Intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)((basket.Items.Sum(item => item.Price*item.Quantity) + shippingCost) * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card"}                   

                };
                paymentIntent = await paymentIntentService.CreateAsync(options); //Integration with Stripe
                basket.paymentIntendId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else //Update Payment Intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)((basket.Items.Sum(item => item.Price*item.Quantity) + shippingCost) * 100)
                };
                paymentIntent = await paymentIntentService.UpdateAsync(basket.paymentIntendId,options);
            }
            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
        {
            var OrderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpec(paymentIntentId, true);
            var order = await OrderRepo.GetEntityWithSpec(spec);
            if (order is null)
                return null;
            if (isPaid)
                order.OrderStatus = OrderStatus.Refunded;
            else
                order.OrderStatus = OrderStatus.Failed;
            OrderRepo.Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
