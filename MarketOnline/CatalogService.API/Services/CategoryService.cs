using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.API.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _repository;
		public CategoryService(ICategoryRepository repository)
		{
			_repository = repository;
		}

		public async Task<Guid> Add(Category category)
		{
			return await _repository.Add(category);
		}

		public void Delete(Guid id)
		{
			_repository.Delete(id);
		}

		public async Task<Category> Get(Guid id)
		{
			return await _repository.Get(id);
		}

		public IEnumerable<Category> List()
		{
			return _repository.List();
		}

		public void Update(Category category)
		{
			_repository.Update(category);
		}
	}
}
