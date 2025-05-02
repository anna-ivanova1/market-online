using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.API.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Product> Add(Product product)
		{
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
			var skip = pageSize * (page == 0 ? 0 : page--);
			return _productRepository
				.List()
				.Where(_ => _.Category.Id == categoryId)
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
