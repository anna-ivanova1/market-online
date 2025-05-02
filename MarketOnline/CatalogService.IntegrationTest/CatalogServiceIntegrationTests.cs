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

			private static Guid categoryId = Guid.Parse("70dfc6f9-2616-4515-be2f-c708a7569e6c");

			[SetUp]
			public void Setup()
			{
				var options = new DbContextOptionsBuilder<CatalogDbContext>()
					.UseInMemoryDatabase(Guid.NewGuid().ToString())
					.Options;

				_dbContext = new CatalogDbContext(options);
				_repository = new ProductRepository(_dbContext);
				_service = new ProductService(_repository);

				var existingCategory = _dbContext.Categories.Find(categoryId);
				if (existingCategory == null)
				{
					_dbContext.Categories.Add(new Category { Id = categoryId, Name = "Category" });
					_dbContext.SaveChanges();
				}
			}

			[TearDown]
			public void TearDown()
			{
				_dbContext.Database.EnsureDeleted();
				_dbContext?.Dispose();
			}

			[Test]
			public async Task Add_ThenGet_ShouldReturnSameProduct()
			{
				var product = CreateTestProduct("Product 5");

				var newProduct = await _service.Add(product);

				Assert.AreEqual(product.Name, newProduct.Name);
				Assert.AreEqual(product.Description, newProduct.Description);
				Assert.AreEqual(product.Amount, newProduct.Amount);

			}

			[Test]
			public async Task Update_ShouldModifyProduct()
			{
				var product = CreateTestProduct("Product 4");
				var newProduct = await _service.Add(product);

				var updated = await _service.Get(newProduct.Id);
				updated.Name = "Updated Name";
				updated.Amount = 99;

				_service.Update(updated);

				var fetched = await _service.Get(newProduct.Id);

				Assert.AreEqual("Updated Name", fetched.Name);
				Assert.AreEqual(99, fetched.Amount);
			}

			[Test]
			public async Task List_ShouldReturnAllAddedProducts()
			{
				await _service.Add(CreateTestProduct("Product 1"));
				await _service.Add(CreateTestProduct("Product 2"));

				var list = _service.List(categoryId, 0, 10).ToList();

				Assert.AreEqual(2, list.Count);
				Assert.IsTrue(list.Any(p => p.Name == "Product 1"));
				Assert.IsTrue(list.Any(p => p.Name == "Product 2"));
			}

			[Test]
			public async Task Delete_ShouldRemoveProduct()
			{
				var product = CreateTestProduct("Product 3");
				var newProduct = await _service.Add(product);

				_service.Delete(newProduct.Id);

				var result = await _service.Get(newProduct.Id);
				Assert.IsNull(result);
			}



			private Product CreateTestProduct(string name)
			{
				var category = _dbContext.Categories.Find(categoryId);

				return new Product
				{
					Name = name,
					Description = "Description",
					Price = new Common.Domain.ValueObjects.Money(10.5, Currency.EUR),
					Category = category,
					Amount = 5
				};
			}
		}
	}

}