using CartService.Application.Entities;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;

namespace CartService.Application.Services
{
	public class CartService(ICartRepository cartRepository, IProductQueryService productQueryService) : ICartService
	{
		private readonly ICartRepository _cartRepository = cartRepository;
		private readonly IProductQueryService _productQueryService = productQueryService;

		public CartDto AddOrUpdate(Cart cart)
		{
			var existing = _cartRepository.Get(cart.Id);

			if (existing == null)
			{
				_cartRepository.Add(cart);
			}
			else
			{
				_cartRepository.Update(cart);
			}

			return GetCart(cart.Id);
		}

		public void Delete(Guid id)
		{
			var existing = _cartRepository.Get(id) ?? throw new ArgumentException(Properties.Resource.CartService_Cart_NotFound);

			_cartRepository.Delete(existing.Id);
		}

		public CartDto Get(Guid id)
		{
			return GetCart(id);
		}

		private CartDto GetCart(Guid id)
		{
			var cart = _cartRepository.Get(id) ?? throw new ArgumentException(Properties.Resource.CartService_Cart_NotFound);

			return new CartDto() { Id = id, Items = cart.Items.Select(GetCartItem).ToList() };
		}

		private CartItemDto GetCartItem(CartItem cartItem)
		{
			var product = _productQueryService.Get(cartItem.Id);

			return product == null
				? throw new ArgumentException(Properties.Resource.CartService_Product_NotFound)
				: new CartItemDto()
				{
					Id = cartItem.Id,
					Quantity = cartItem.Quantity,
					Name = product.Name,
					Price = product.Price,
					Image = product.Image,
				};
		}
	}
}
