using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces
{
	public interface IProductRepository : IRepository<Product, int>
	{
	}
}
