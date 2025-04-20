using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces
{
	public interface ICategoryService
	{
		public Task<Category> Get(Guid id);

		public IEnumerable<Category> List();

		public Task<Guid> Add(Category category);

		public void Update(Category category);

		public void Delete(Guid id);

	}
}
