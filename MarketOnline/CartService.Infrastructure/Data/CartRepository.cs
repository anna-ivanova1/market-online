using CartService.Application.Interfaces;
using CartService.Domain.Entities;

namespace CartService.Infrastructure.Data
{
	public class CartRepository : PersistentStorage<Guid, Cart>, ICartRepository
	{
		public CartRepository(string name) : base(name, "Carts") { }
	}
}
