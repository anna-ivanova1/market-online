using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Infrastructure.Data
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly CatalogDbContext _context;

		public CategoryRepository(CatalogDbContext constext)
		{
			_context = constext;
		}

		public async Task<Guid> Add(Category item)
		{
			_context.Categories.Add(item);

			await _context.SaveChangesAsync();

			return item.Id;
		}

		public async Task Delete(Guid id)
		{
			var entity = await _context.Categories.FindAsync([id]);

			if (entity != null)
			{
				_context.Categories.Remove(entity);

				await _context.SaveChangesAsync();
			}
		}

		public async Task<Category> Get(Guid id)
		{
			return await _context.Categories.FindAsync([id]);
		}

		public IEnumerable<Category> List()
		{
			return _context.Categories.AsQueryable();
		}

		public async Task Update(Category item)
		{
			var entity = await _context.Categories.FindAsync([item.Id]);

			if (entity != null)
			{
				item.CopyTo(entity);

				await _context.SaveChangesAsync();
			}
		}
	}
}
