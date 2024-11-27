using Shopping.Core.IServices;
using Shopping.Service.AuthService;
using Shopping.Service.CacheService;
using Shopping.Service.OrderService;
using Shopping.Service.PaymentService;
using Shopping.Service.ProductService;

namespace RouteWebAPI.Config
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ServicesConfigurationEX(this IServiceCollection services)
        {
            // Add your service configurations here
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddSingleton(typeof(ICacheResponseService), typeof(CacheResponseService));

            return services;
        }
    }
}
