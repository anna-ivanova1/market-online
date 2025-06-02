using CartService.Domain.Entities;

namespace CartService.Domain.Interfaces
{
	public interface ICartService
	{
		IEnumerable<Cart> List();

		Cart Get(Guid id);

		Cart AddOrUpdateCartItem(Guid id, CartItem item);

		Cart UpdateCartItemQuantity(Guid id, int itemId, int quantity);

		bool DeleteCartItem(Guid id, int itemId);

		void UpdateCartItems(int id, string name, Money price);
	}
}
