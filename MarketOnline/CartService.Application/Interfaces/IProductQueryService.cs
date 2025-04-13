using ProductService.Application.Entities;

namespace CartService.Application.Interfaces
{
	public interface IProductQueryService
	{
		ProductShortDto Get(int productId);
	}
}
