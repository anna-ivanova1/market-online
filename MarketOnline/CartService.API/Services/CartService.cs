using CartService.Domain.Entities;
using CartService.Domain.Interfaces;

namespace CartService.API.Services
{
	/// <summary>
	/// Cart service
	/// </summary>
	/// <param name="cartRepository"></param>
	public class CartService(ICartRepository cartRepository) : ICartService
	{
		private readonly ICartRepository _cartRepository = cartRepository;

		/// <summary>
		/// List all the carts
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Cart> List()
		{
			return _cartRepository.List();
		}

		/// <summary>
		/// Adds or updates the cart item
		/// </summary>
		/// <param name="id">Id</param>
		/// <param name="item">Item</param>
		/// <returns>New or updated cart</returns>
		public Cart AddOrUpdateCartItem(Guid id, CartItem item)
		{
			var existingCart = _cartRepository.GetById(id) ?? new Cart() { Id = id };

			existingCart.AddOrUpdateItem(item);

			_cartRepository.Upsert(existingCart);

			return existingCart;
		}

		/// <summary>
		/// Deletes cart item
		/// </summary>
		/// <param name="id">Id</param>
		/// <param name="itemId">Item id</param>
		/// <returns>TRUE - if succeeded</returns>
		public bool DeleteCartItem(Guid id, int itemId)
		{
			var existingCart = _cartRepository.GetById(id);

			if (existingCart != null && existingCart.DeleteItem(itemId))
			{
				_cartRepository.Upsert(existingCart);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets a cart by Id
		/// </summary>
		/// <param name="id">id</param>
		/// <returns>Resulted cart</returns>
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

		/// <summary>
		/// Updates cart item quantity
		/// </summary>
		/// <param name="id">id</param>
		/// <param name="itemId">id of the item</param>
		/// <param name="quantity">quantity</param>
		/// <returns>Resulted cart</returns>
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

		/// <summary>
		/// Updates cart items
		/// </summary>
		/// <param name="id">id</param>
		/// <param name="name">name</param>
		/// <param name="price">price</param>
		public void UpdateCartItems(int id, string name, Money price)
		{
			_cartRepository.UpdateCartItems(id, name, price);
		}
	}
}
