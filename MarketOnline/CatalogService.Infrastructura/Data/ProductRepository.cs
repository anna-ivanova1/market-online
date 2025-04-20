using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Infrastructure.Data
{
	public class ProductRepository : IProductRepository
	{
		private readonly CatalogDbContext _context;

		public ProductRepository(CatalogDbContext constext)
		{
			_context = constext;
		}

		public async Task<int> Add(Product item)
		{
			_context.Products.Add(item);

			await _context.SaveChangesAsync();

			return item.Id;
		}

		public async Task Delete(int id)
		{
			var entity = await _context.Products.FindAsync([id]);

			if (entity != null)
			{
				_context.Products.Remove(entity);

				await _context.SaveChangesAsync();
			}
		}

		public async Task<Product> Get(int id)
		{
			return await _context.Products.FindAsync([id]);
		}

		public IEnumerable<Product> List()
		{
			return _context.Products.AsQueryable();
		}

		public async Task Update(Product item)
		{
			var entity = await _context.Products.FindAsync([item.Id]);

			if (entity != null)
			{
				item.CopyTo(entity);

				await _context.SaveChangesAsync();
			}
		}
	}
}
