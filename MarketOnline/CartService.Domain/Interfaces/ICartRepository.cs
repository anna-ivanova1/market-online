using CartService.Domain.Entities;

namespace CartService.Domain.Interfaces
{
	public interface ICartRepository
	{
		void Upsert(Cart cart);

		Cart? GetById(Guid id);

		bool Delete(Guid id);
	}
}
