using CartService.Domain.Entities;

namespace CartService.Domain.Interfaces
{
	public interface ICartRepository
	{
		IEnumerable<Cart> List();

		void Upsert(Cart cart);

		Cart? GetById(Guid id);

		bool Delete(Guid id);

		void UpdateCartItems(int id, string name, Money price);
	}
}
