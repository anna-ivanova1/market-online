namespace CartService.APITests
{
	using CartService.API.Models;
	using NUnit.Framework;
	using System.Net.Http.Json;
	using System.Text.Json;

	[TestFixture]
	public class CartControllerV2Tests
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
		public async Task Get_ReturnsCartItems()
		{
			var price = new MoneyModel() { Currency = "EUR", Amount = 2 };
			var cartId = Guid.NewGuid().ToString();
			var item = new CartItemModel { Id = 3, Quantity = 5, Name = "CartItem_1", Price = price };

			var responsePost = await _client.PostAsJsonAsync($"/api/v2/Cart/{cartId}/items", item);

			Assert.That(responsePost.IsSuccessStatusCode);

			// Act
			var response = await _client.GetAsync($"/api/v2.0/Cart/{cartId}");

			// Assert
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			var items = JsonSerializer.Deserialize<List<CartItemModel>>(json, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);
			Assert.AreEqual(3, items[0].Id);
			Assert.AreEqual(5, items[0].Quantity);

			var responseDelete = await _client.DeleteAsync($"/api/v2/Cart/{cartId}/items/{item.Id}");

			Assert.That(responseDelete.IsSuccessStatusCode);

			var responseGet = await _client.GetAsync($"/api/v2.0/Cart/{cartId}");

			responseGet.EnsureSuccessStatusCode();

			var json2 = await responseGet.Content.ReadAsStringAsync();
			var items2 = JsonSerializer.Deserialize<List<CartItemModel>>(json2, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			Assert.IsNotNull(items2);
			Assert.AreEqual(0, items2.Count);
		}

		[Test]
		public async Task AddItem_ReturnsOk()
		{
			var price = new MoneyModel() { Currency = "EUR", Amount = 2 };
			var cartId = Guid.NewGuid().ToString();
			var item = new CartItemModel { Id = 3, Quantity = 1, Name = "CartItem_1", Price = price };

			var response = await _client.PostAsJsonAsync($"/api/v2/Cart/{cartId}/items", item);

			Assert.That(response.IsSuccessStatusCode);
		}
	}

}