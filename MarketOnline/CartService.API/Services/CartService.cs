using CartService.Domain.Entities;
using CartService.Domain.Interfaces;

namespace CartService.API.Services
{
	public class CartService(ICartRepository cartRepository) : ICartService
	{
		private readonly ICartRepository _cartRepository = cartRepository;

		public IEnumerable<Cart> List()
		{
			return _cartRepository.List();
		}

		public Cart AddOrUpdateCartItem(Guid id, CartItem item)
		{
			var existingCart = _cartRepository.GetById(id) ?? new Cart() { Id = id };

			existingCart.AddOrUpdateItem(item);

			_cartRepository.Upsert(existingCart);

			return existingCart;
		}

		public bool DeleteCartItem(Guid id, int itemId)
		{
			var existingCart = _cartRepository.GetById(id);

			var result = existingCart != null ? existingCart.DeleteItem(itemId) : false;

			if (result)
			{
				_cartRepository.Upsert(existingCart);
			}

			return result;
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

		public void UpdateCartItems(int id, string name, Money price)
		{
			_cartRepository.UpdateCartItems(id, name, price);
		}
	}
}
