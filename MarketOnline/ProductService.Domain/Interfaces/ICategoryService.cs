using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces
{
	public interface ICategoryService
	{
		public Task<Category> Get(Guid id);

		public IEnumerable<Category> List();

		public Task<Category> Add(Category category);

		public Task<Category> Update(Category category);

		public Task<bool> Delete(Guid id);

	}
}
