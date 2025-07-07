using AutoMapper;

using CartService.API.Models;
using CartService.Domain.Entities;

using Common.Domain.Entities;

namespace CartService.API
{
	/// <summary>
	/// General mapping profile of CartService
	/// </summary>
	public class MappingProfile : Profile
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="MappingProfile"/> class.
		/// </summary>
		public MappingProfile()
		{
			CreateMap<Image, ImageModel>().ReverseMap();
			CreateMap<Money, MoneyModel>().ReverseMap();
			CreateMap<CartItem, CartItemModel>().ReverseMap();
			CreateMap<Cart, CartModel>().ReverseMap();
		}
	}
}
