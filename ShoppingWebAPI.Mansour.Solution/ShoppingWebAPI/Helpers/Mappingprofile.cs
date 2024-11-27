using AutoMapper;
using RouteWebAPI.Dtos;
using RouteWebAPI.Dtos.BasketDto;
using RouteWebAPI.Dtos.OrderDto;
using Shopping.Core.Models;
using Shopping.Core.Models.Basket;
using Shopping.Core.Models.OrderComponents;
using UsesrAddress = Shopping.Core.IdentityModels.Address;
namespace RouteWebAPI.Helpers
{
    public class Mappingprofile :Profile
    {
        public Mappingprofile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                .ForMember(dest => dest.Category,opt => opt.MapFrom(src => src.Category !=null ? src.Category.Name:null))
                .ForMember(dest => dest.PictureUrl,opt => opt.MapFrom<ProductPictureUrlResolver>());
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Address>();
            CreateMap<AddressDto, UsesrAddress>();
            CreateMap<Order, OrderResponseDto>()
                .ForMember(d => d.DeliveryMethod,O => O.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(s => s.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.ProductImage, O => O.MapFrom(S => S.Product.ProductImage))
                .ForMember(d=>d.ProductImage, O =>O.MapFrom<OrderItemPictureUrlResolver>());


        }
    }
}
