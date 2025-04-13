using CartService.Domain.Entities;
using Common.Domain.Interfaces;

namespace CartService.Application.Interfaces
{
	public interface ICartRepository : IPersistentStorage<Guid, Cart> { }
}
