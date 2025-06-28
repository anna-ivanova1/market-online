using CatalogService.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace CatalogService.APITests
{
	internal class ProductAPITests
	{
		[TestFixture]
		public class ProductsControllerTests
		{
			private CustomWebApplicationFactory _factory;
			private HttpClient _client;

			private readonly Guid Category_ID = Guid.Parse("9f8bcd1e6a1f47a485b64c8a3e88c6ca");

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
			public async Task Get_WithEmptyDb_ReturnsEmptyList()
			{
				var response = await _client.GetAsync($"/api/catalog-service/products?categoryId={Category_ID}&pageNumber=1&pageSize=5");

				Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
				var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
				Assert.IsNotNull(products);
				Assert.IsEmpty(products);
			}

			[Test]
			public async Task Post_And_GetById_WorksCorrectly()
			{
				var newCategory = GetCategory();
				_client.PostAsJsonAsync("/api/catalog-service/categories", newCategory);

				var newProduct = GetProduct();

				var post = await _client.PostAsJsonAsync("/api/catalog-service/products", newProduct);
				Assert.AreEqual(HttpStatusCode.OK, post.StatusCode);

				var created = await post.Content.ReadFromJsonAsync<ProductModel>();
				Assert.IsNotNull(created);
				Assert.AreEqual("New Product", created.Name);

				var get = await _client.GetAsync($"/api/catalog-service/products/{created.Id}");
				Assert.AreEqual(HttpStatusCode.OK, get.StatusCode);

				var fetched = await get.Content.ReadFromJsonAsync<ProductModel>();
				Assert.IsNotNull(fetched);
				Assert.AreEqual(created.Id, fetched.Id);

				var deleteResponce = await _client.DeleteAsync($"/api/catalog-service/products/{created.Id}");

				Assert.AreEqual(HttpStatusCode.OK, deleteResponce.StatusCode);
			}

			private CategoryModel GetCategory()
			{
				return new CategoryModel() { Name = "Test Category", Id = Category_ID };
			}

			private ProductModel GetProduct()
			{
				return new ProductModel
				{
					Name = "New Product",
					Description = "Some description",
					Price = new MoneyModel()
					{
						Amount = 6,
						Currency = "EUR"
					},
					Amount = 1,
					CategoryId = Category_ID
				};
			}
		}
	}
}
