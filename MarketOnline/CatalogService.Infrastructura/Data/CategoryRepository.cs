using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Data
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly CatalogDbContext _context;

		public CategoryRepository(CatalogDbContext context)
		{
			_context = context;
		}

		public async Task<Guid> Add(Category item)
		{
			_context.Categories.Add(item);

			await _context.SaveChangesAsync();

			return item.Id;
		}

		public async Task<bool> Delete(Guid id)
		{
			var entity = await _context.Categories.FindAsync([id]);

			if (entity != null)
			{
				var products = _context.Products.Where(_ => _.Category.Id == id);
				_context.Products.RemoveRange(products);

				_context.Categories.Remove(entity);

				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<Category> Get(Guid id)
		{
			return await _context.Categories.Include(_ => _.Parent).Where(_ => _.Id == id).FirstAsync();
		}

		public IQueryable<Category> List()
		{
			return _context.Categories.AsQueryable();
		}

		public async Task Update(Category item)
		{
			var entity = await _context.Categories.FindAsync([item.Id]);

			if (entity == null) return;

			item.CopyTo(entity);

			await _context.SaveChangesAsync();
		}
	}
}
