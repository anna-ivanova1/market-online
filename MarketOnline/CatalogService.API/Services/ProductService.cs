using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.API.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly ICategoryRepository _categoryRepository;

		public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}

		public async Task<Product> Add(Product product)
		{
			var category = await _categoryRepository.Get(product.CategoryId);
			product.Category = category;
			var id = await _productRepository.Add(product);
			return await _productRepository.Get(id);
		}

		public async Task<bool> Delete(int id)
		{
			return await _productRepository.Delete(id);
		}

		public async Task<Product> Get(int id)
		{
			return await _productRepository.Get(id);
		}

		public IEnumerable<Product> List(Guid categoryId, int page, int pageSize)
		{
			var skip = pageSize * (page == 1 ? 0 : page--);
			var products = _productRepository
				.List().ToList();
			return products
				.Where(_ => _.CategoryId == categoryId)
				.OrderBy(_ => _.Name)
				.Skip(skip)
				.Take(pageSize);
		}

		public async Task<Product> Update(Product product)
		{
			await _productRepository.Update(product);

			return await _productRepository.Get(product.Id);
		}
	}
}
