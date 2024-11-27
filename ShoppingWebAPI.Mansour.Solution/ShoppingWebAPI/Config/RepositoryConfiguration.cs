using Shopping.Core;
using Shopping.Core.IRepository;
using Shopping.Repository;
using Shopping.Repository.BasketRepository;
using Shopping.Repository.UnitOfWork;

namespace RouteWebAPI.Config
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection RepositoryConfigurationEX(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            return services;
        }
    }
}
