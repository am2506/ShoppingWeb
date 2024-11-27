using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using RouteWebAPI.Dtos.OrderDto;
using Shopping.Core.Models.OrderComponents;

namespace RouteWebAPI.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!source.Product.ProductImage.IsNullOrEmpty())
            {
                return $"{_configuration["APIBaseUrl"]}/{source.Product.ProductImage}";
            }
            return string.Empty;
        }
    }
}
