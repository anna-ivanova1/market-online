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

		public async Task<int> Add(Product product)
		{
			return await _productRepository.Add(product);
		}

		public void Delete(int id)
		{
			_productRepository?.Delete(id);
		}

		public async Task<Product> Get(int id)
		{
			return await _productRepository.Get(id);
		}

		public IEnumerable<Product> List()
		{
			return _productRepository.List();
		}

		public void Update(Product product)
		{
			_productRepository.Update(product);
		}
	}
}
