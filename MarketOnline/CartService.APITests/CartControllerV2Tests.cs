
using System.Net.Http.Json;
using System.Text.Json;

using CartService.API.Models;

namespace CartService.APITests
{

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
			Assert.That(items.Count, Is.EqualTo(1));
			Assert.That(items[0].Id, Is.EqualTo(3));
			Assert.That(items[0].Quantity, Is.EqualTo(5));

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
			Assert.That(items2.Count, Is.EqualTo(0));
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
