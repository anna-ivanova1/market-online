using CatalogService.API.Models;
using CatalogService.APITests;
using System.Net;
using System.Net.Http.Json;

namespace CatalogService.Tests.Integration
{
	[TestFixture]
	public class CategoriesControllerTests
	{
		private CustomWebApplicationFactory _factory;
		private HttpClient _client;

		[SetUp]
		public void SetUp()
		{
			_factory = new CustomWebApplicationFactory();
			_client = _factory.CreateClient();
		}

		[TearDown]
		public void TearDown()
		{
			_client.Dispose();
			_factory.Dispose();
		}

		[Test]
		public async Task Get_ShouldReturnAllCategories()
		{
			// Act
			var response = await _client.GetAsync("/api/catalog-service/categories");

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var categories = await response.Content.ReadFromJsonAsync<List<CategoryModel>>();
			Assert.IsNotNull(categories);
		}

		[Test]
		public async Task GetById_ShouldReturnCategory()
		{
			// Arrange
			var testCategoryId = await CreateTestCategoryAsync("Test Category");

			// Act
			var response = await _client.GetAsync($"/api/catalog-service/categories/{testCategoryId}");

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var category = await response.Content.ReadFromJsonAsync<CategoryModel>();
			Assert.IsNotNull(category);
			Assert.AreEqual(testCategoryId, category!.Id);
		}

		[Test]
		public async Task Add_ShouldCreateCategory()
		{
			// Arrange
			var newCategory = new CategoryModel
			{
				Name = "Integration Category"
			};

			// Act
			var response = await _client.PostAsJsonAsync("/api/catalog-service/categories", newCategory);

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var created = await response.Content.ReadFromJsonAsync<CategoryModel>();
			Assert.IsNotNull(created);
			Assert.AreEqual("Integration Category", created!.Name);
		}

		[Test]
		public async Task Update_ShouldModifyCategory()
		{
			// Arrange
			var categoryId = await CreateTestCategoryAsync("To Be Updated");

			var updated = new CategoryModel
			{
				Id = categoryId,
				Name = "Updated Name"
			};

			// Act
			var response = await _client.PutAsJsonAsync("/api/catalog-service/categories", updated);

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var result = await response.Content.ReadFromJsonAsync<CategoryModel>();
			Assert.IsNotNull(result);
			Assert.AreEqual("Updated Name", result!.Name);
		}

		[Test]
		public async Task Delete_ShouldRemoveCategory()
		{
			// Arrange
			var id = await CreateTestCategoryAsync("To Be Deleted");

			// Act
			var response = await _client.DeleteAsync($"/api/catalog-service/categories/{id}");

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

			// Optionally verify it's gone
			var check = await _client.GetAsync($"/api/catalog-service/categories/{id}");
			Assert.AreNotEqual(HttpStatusCode.OK, check.StatusCode); // Expect NotFound or InternalServerError
		}

		private async Task<Guid> CreateTestCategoryAsync(string name)
		{
			var model = new CategoryModel { Name = name };
			var response = await _client.PostAsJsonAsync("/api/catalog-service/categories", model);
			var created = await response.Content.ReadFromJsonAsync<CategoryModel>();
			return created!.Id;
		}
	}
}
