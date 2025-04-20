namespace CatalogService.IntegrationTest
{
	using Common.Domain.Enums;
	using global::CatalogService.API.Services;
	using global::CatalogService.Domain.Entities;
	using global::CatalogService.Domain.Interfaces;
	using global::CatalogService.Infrastructure.Data;
	using Microsoft.EntityFrameworkCore;
	using NUnit.Framework;
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	namespace CatalogService.IntegrationTests
	{
		public class ProductServiceIntegrationTests
		{
			private CatalogDbContext _dbContext;
			private IProductRepository _repository;
			private IProductService _service;

			[SetUp]
			public void Setup()
			{
				var options = new DbContextOptionsBuilder<CatalogDbContext>()
					.UseInMemoryDatabase(Guid.NewGuid().ToString())
					.Options;

				_dbContext = new CatalogDbContext(options);
				_repository = new ProductRepository(_dbContext);
				_service = new ProductService(_repository);
			}

			[TearDown]
			public void TearDown()
			{
				_dbContext?.Dispose();
			}

			[Test]
			public async Task Add_ThenGet_ShouldReturnSameProduct()
			{
				var product = CreateTestProduct();

				var id = await _service.Add(product);
				var fetched = await _service.Get(id);

				Assert.AreEqual(product.Name, fetched.Name);
				Assert.AreEqual(product.Description, fetched.Description);
				Assert.AreEqual(product.Amount, fetched.Amount);
			}

			[Test]
			public async Task Update_ShouldModifyProduct()
			{
				var product = CreateTestProduct();
				var id = await _service.Add(product);

				var updated = await _service.Get(id);
				updated.Name = "Updated Name";
				updated.Amount = 99;

				_service.Update(updated);

				var fetched = await _service.Get(id);

				Assert.AreEqual("Updated Name", fetched.Name);
				Assert.AreEqual(99, fetched.Amount);
			}

			[Test]
			public async Task Delete_ShouldRemoveProduct()
			{
				var product = CreateTestProduct();
				var id = await _service.Add(product);

				_service.Delete(id);

				var result = await _service.Get(id);
				Assert.IsNull(result);
			}

			[Test]
			public async Task List_ShouldReturnAllAddedProducts()
			{
				await _service.Add(CreateTestProduct("Product 1"));
				await _service.Add(CreateTestProduct("Product 2"));

				var list = _service.List().ToList();

				Assert.AreEqual(2, list.Count);
				Assert.IsTrue(list.Any(p => p.Name == "Product 1"));
				Assert.IsTrue(list.Any(p => p.Name == "Product 2"));
			}

			private Product CreateTestProduct(string name = "Test Product")
			{
				return new Product
				{
					Name = name,
					Description = "Description",
					Price = new Common.Domain.ValueObjects.Money(10.5, Currency.EUR),
					Category = new Category { Id = Guid.NewGuid(), Name = "Category" },
					Amount = 5
				};
			}
		}
	}

}