using CartService.Application.Entities;
using CartService.Domain.Entities;

namespace CartService.Application.Interfaces
{
	public interface ICartService
	{
		CartDto Get(Guid id);

		CartDto AddOrUpdate(Cart cart);

		void Delete(Guid id);
	}
}
