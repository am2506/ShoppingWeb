using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core;
using Shopping.Core.IRepository;
using Shopping.Core.IServices;
using Shopping.Core.Models;
using Shopping.Core.Models.OrderComponents;
using Shopping.Core.Specification;
using Shopping.Repository.SpecificationDesignPattern;

namespace Shopping.Service.OrderService
{
    public class OrderService : IOrderService
    {

        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenericRepository<Order> _orderRepo;
        ///public OrderService(IBasketRepository basketRepository,
        ///    IGenericRepository<Product> productRepo,
        ///    IGenericRepository<DeliveryMethod> deliveryMethodRepo,
        ///    IGenericRepository<Order> orderRepo)
        ///{
        ///   //_basketRepository = basketRepository;
        ///   //_productRepo = productRepo;
        ///   //_productRepo = productRepo;
        ///   // _deliveryMethodRepo = deliveryMethodRepo;
        ///   //_orderRepo = orderRepo;
        ///}
        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, Address address, int deliveryId)
        {
            
            // 1.Get Basket From Baskets Repo
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            if (Basket is null)
                return null;
            // 2. Get Selected Items at Basket From Products Repo
            //Each Items in basket will assign to orderItem
            var orderItems = new List<OrderItem>();
            if (Basket?.Items?.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                    if (product is not null)
                    {
                    var ProductItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(ProductItem, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                    }
                    else
                    {
                        var ProductItem = new ProductItemOrder(-1, string.Empty, string.Empty);
                        var orderItem = new OrderItem(ProductItem, 0m, item.Quantity);
                        orderItems.Add(orderItem);
                    }
                                           
                }
            }
            if (orderItems?.Count <= 0)
                return new Order();
            // 3. Calculate SubTotal
            var subtotal = orderItems.Sum(Item => Item.Price * Item.Quantity);
            // 4. Get Delivery Method From DeliveryMethods Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryId);

            //Validation To Check if payment intent don't has other order
            var spec = new OrderSpec(Basket.paymentIntendId,true);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if(existingOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            }
            
            // 5. Create Order
            var order = new Order()
            {
                BuyerEmail = buyerEmail,
                OrderItems = orderItems,
                ShippingAddress = address,
                DeliveryMethod = deliveryMethod,
                SubTotal = subtotal,
                PaymentIntentId = Basket?.paymentIntendId??string.Empty
            };
            await _unitOfWork.Repository<Order>().AddAsync(order);
            // 6. Save To Database [TODO]
            await _unitOfWork.CompleteAsync();
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods.ToList().AsReadOnly();
        }

        public async Task<Order?> GetUserOrderByOrderIdAsync(string email, int OrderId)
        {
            var spec = new OrderSpec(email, OrderId);
            var userOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            return userOrder is not null?userOrder:null;
        }

        public async Task<ICollection<Order>> GetUserOrdersByEmailAsync(string email)
        {
            var spec = new OrderSpec(email);
           var userOrders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return userOrders.ToList();
        }
    }
}
