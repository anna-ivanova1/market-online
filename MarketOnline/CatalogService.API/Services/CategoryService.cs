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

		public async Task<Category?> Add(Category category)
		{
			var id = await _repository.Add(category);
			return await _repository.Get(id);
		}

		public async Task<bool> Delete(Guid id)
		{
			return await _repository.Delete(id);
		}

		public async Task<Category?> Get(Guid id)
		{
			return await _repository.Get(id);
		}

		public IEnumerable<Category> List()
		{
			return _repository.List();
		}

		public async Task<Category?> Update(Category category)
		{
			await _repository.Update(category);

			return await _repository.Get(category.Id);
		}
	}
}
