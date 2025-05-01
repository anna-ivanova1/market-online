using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces
{
	public interface IProductService
	{
		public Task<Product> Get(int id);

		public IEnumerable<Product> List();

		public Task<int> Add(Product product);

		public void Update(Product product);

		public void Delete(int id);
	}
}
