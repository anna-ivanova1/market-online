using CartService.Application.Interfaces;
using Common.Domain.ValueObjects;
using ProductService.Application.Entities;

namespace CartService.Application.IntegrationTests.Mocks
{
	public class MockProductQueryService : IProductQueryService
	{
		private readonly Dictionary<int, ProductShortDto> _products = [];

		public MockProductQueryService()
		{
			_products.Add(1, new ProductShortDto()
			{
				Id = 1,
				Name = "Product",
				Price = new Money(11.22, Common.Domain.Enums.Currency.USD)
			});

		}

		public ProductShortDto Get(int id) => _products.TryGetValue(id, out var p) ? p : throw new ArgumentException("No product");
	}
}
