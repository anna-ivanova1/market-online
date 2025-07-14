namespace CatalogService.IntegrationTest
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	using Common.Domain.Enums;

	using global::CatalogService.API.Services;
	using global::CatalogService.Domain.Entities;
	using global::CatalogService.Domain.Interfaces;
	using global::CatalogService.Infrastructure.Data;
	using global::CatalogService.Infrastructure.Publishers;

	using Microsoft.EntityFrameworkCore;

	using NUnit.Framework;

	namespace CatalogService.IntegrationTests
	{
		public class ProductServiceIntegrationTests
		{
			private CatalogDbContext _dbContext;
			private IProductRepository _repository;
			private ICategoryRepository _categoryRepository;
			private IProductService _service;
			private IProductUpdatePublisher _updatePublisher;

			private static Guid categoryId = Guid.Parse("70dfc6f9-2616-4515-be2f-c708a7569e6c");

			[SetUp]
			public void Setup()
			{
				var options = new DbContextOptionsBuilder<CatalogDbContext>()
					.UseInMemoryDatabase(Guid.NewGuid().ToString())
					.Options;

				_dbContext = new CatalogDbContext(options);
				_repository = new ProductRepository(_dbContext);
				_categoryRepository = new CategoryRepository(_dbContext);
				_updatePublisher = new ProductUpdatePublisher();
				_service = new ProductService(_repository, _categoryRepository, _updatePublisher);

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

				Assert.That(newProduct?.Name, Is.EqualTo(product.Name));
				Assert.That(newProduct?.Description, Is.EqualTo(product.Description));
				Assert.That(newProduct?.Amount, Is.EqualTo(product.Amount));

			}

			[Test]
			public async Task Update_ShouldModifyProduct()
			{
				var product = CreateTestProduct("Product 4");
				var newProduct = await _service.Add(product);

				if (newProduct != null)
				{
					var updated = await _service.Get(newProduct.Id);
					if (updated != null)
					{
						updated.Name = "Updated Name";
						updated.Amount = 99;

						await _service.Update(updated);

						var fetched = await _service.Get(newProduct.Id);

						Assert.That(fetched?.Name, Is.EqualTo("Updated Name"));
						Assert.That(fetched?.Amount, Is.EqualTo(99));
					}
				}
			}

			[Test]
			public async Task List_ShouldReturnAllAddedProducts()
			{
				await _service.Add(CreateTestProduct("Product 1"));
				await _service.Add(CreateTestProduct("Product 2"));

				var list = _service.List(categoryId, 0, 10).ToList();

				Assert.That(list.Count, Is.EqualTo(2));
				Assert.IsTrue(list.Any(p => p.Name == "Product 1"));
				Assert.IsTrue(list.Any(p => p.Name == "Product 2"));
			}

			[Test]
			public async Task Delete_ShouldRemoveProduct()
			{
				var product = CreateTestProduct("Product 3");
				var result = await _service.Add(product);

				if (result != null)
				{
					await _service.Delete(result.Id);
					result = await _service.Get(result.Id);

					Assert.IsNull(result);
				}
			}

			private Product CreateTestProduct(string name)
			{
				var category = _dbContext.Categories.Find(categoryId) ?? new Category() { Name = "TestCategory", Id = categoryId };

				return new Product
				{
					Name = name,
					Description = "Description",
					Price = new Money() { Amount = 10.5, Currency = Currency.EUR },
					Category = category,
					CategoryId = categoryId,
					Amount = 5
				};
			}
		}
	}

}
