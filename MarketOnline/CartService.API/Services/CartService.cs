using CartService.Domain.Entities;
using CartService.Domain.Interfaces;

namespace CartService.API.Services
{
	public class CartService(ICartRepository cartRepository) : ICartService
	{
		private readonly ICartRepository _cartRepository = cartRepository;

		public Cart AddOrUpdateCartItem(Guid id, CartItem item)
		{
			var existingCart = _cartRepository.GetById(id) ?? new Cart() { Id = id };

			var existingCartItem = existingCart.Items.FirstOrDefault(_ => _.Id == item.Id);

			existingCart.AddOrUpdateItem(item);

			_cartRepository.Upsert(existingCart);

			return existingCart;
		}

		public void DeleteCartItem(Guid id, int itemId)
		{
			var existingCart = _cartRepository.GetById(id);

			if (existingCart != null)
			{
				existingCart.DeleteItem(itemId);
			}
		}

		public Cart Get(Guid id)
		{
			var foundCart = _cartRepository.GetById(id);

			if (foundCart != null)
			{
				return foundCart;
			}

			var newCart = new Cart { Id = id };

			_cartRepository.Upsert(newCart);

			return newCart;
		}

		public Cart UpdateCartItemQuantity(Guid id, int itemId, int quantity)
		{
			var existingCart = _cartRepository.GetById(id) ?? new Cart() { Id = id };

			existingCart.UpdateItemQuantity(itemId, quantity);

			if (existingCart.Items.Count == 0)
			{
				_cartRepository.Delete(id);
			}
			else
			{
				_cartRepository.Upsert(existingCart);
			}

			return existingCart;
		}
	}
}
