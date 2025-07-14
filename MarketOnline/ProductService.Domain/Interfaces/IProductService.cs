using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces
{
	public interface IProductService
	{
		public Task<Product?> Get(int id);

		public IEnumerable<Product> List(Guid categoryId, int page, int pageSize);

		public Task<Product?> Add(Product product);

		public Task<Product?> Update(Product product);

		public Task<bool> Delete(int id);
	}
}
