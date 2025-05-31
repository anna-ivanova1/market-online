using AutoMapper;
using CartService.API.Models;
using CartService.Domain.Entities;
using Common.Domain.Entities;

namespace CartService.API
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Image, ImageModel>().ReverseMap();
			CreateMap<Money, MoneyModel>().ReverseMap();
			CreateMap<CartItem, CartItemModel>().ReverseMap();
			CreateMap<Cart, CartModel>().ReverseMap();
		}
	}
}
