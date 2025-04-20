using CartService.Domain.Entities;

namespace CartService.Domain.Interfaces
{
	public interface ICartService
	{
		Cart Get(Guid id);

		Cart AddOrUpdateCartItem(Guid id, CartItem item);

		Cart UpdateCartItemQuantity(Guid id, int itemId, int quantity);

		void DeleteCartItem(Guid id, int itemId);
	}
}
