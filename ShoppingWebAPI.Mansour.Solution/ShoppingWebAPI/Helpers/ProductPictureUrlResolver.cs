using AutoMapper;
using RouteWebAPI.Dtos;
using Shopping.Core.Models;

namespace RouteWebAPI.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
           if(!string.IsNullOrEmpty(source.Name))
            {
                return $"{_configuration["APIBaseUrl"]}/{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
