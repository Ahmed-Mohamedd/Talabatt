using AutoMapper;
using SmartCart.Api.Dtos;
using Talabat.Api.DTOs;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Entites.OrderAggregate;
using Talabat.DAL.Entities;

namespace Talabat.Api.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
              .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
              .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
              .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Address, AddressDto>()
                .ForMember(d=> d.LastName , o => o.MapFrom(s => s.SecondName));
            CreateMap<AddressDto, Address>()
                .ForMember(d => d.SecondName, o => o.MapFrom(s => s.LastName));

            CreateMap<OrderAddress , AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}
