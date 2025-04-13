using CartService.Application.IntegrationTests.Mocks;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;
using CartService.Infrastructure.Data;

namespace CartService.Application.IntegrationTests.Service
{
	public class CartServiceIntegrationTests
	{
		private MockProductQueryService _productService;
		private ICartRepository _cartRepository;
		private Services.CartService _service;

		[SetUp]
		public void Setup()
		{
			_cartRepository = new CartRepository(":memory:");
			_productService = new MockProductQueryService();
			_service = new Services.CartService(_cartRepository, _productService);
		}

		[Test]
		public void AddOrUpdate_SavesCartInLiteDB()
		{
			var cartId = Guid.NewGuid();
			var cart = new Cart
			{
				Id = cartId,
			};

			cart.AddItem(new CartItem { Id = 1, Quantity = 3 });

			var result = _service.AddOrUpdate(cart);

			var loadedCart = _cartRepository.Get(cartId);
			Assert.NotNull(loadedCart);
			Assert.That(loadedCart.Items.Count, Is.EqualTo(1));
			Assert.That(loadedCart.Items.Where(_ => _.Id == 1).FirstOrDefault()?.Quantity, Is.EqualTo(3));
		}
	}

}
