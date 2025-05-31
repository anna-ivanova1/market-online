using CartService.Domain.Entities;
using CartService.Infrastructure.Data;
using Common.Domain.Entities;
using Common.Domain.Enums;

namespace CartService.IntegrationTests
{
	[TestFixture]
	public class CartServiceIntegrationTests
	{
		private string _testDbPath;
		private CartRepository _repository;
		private API.Services.CartService _service;

		[SetUp]
		public void Setup()
		{
			_testDbPath = $"CartTest_{Guid.NewGuid()}.db";
			_repository = new CartRepository(_testDbPath);
			_service = new API.Services.CartService(_repository);
		}

		[TearDown]
		public void Cleanup()
		{
			if (File.Exists(_testDbPath))
			{
				File.Delete(_testDbPath);
			}
		}

		private CartItem CreateCartItem(int id, string name = "Test", int quantity = 1, double price = 10)
		{
			return new CartItem
			{
				Id = id,
				Name = name,
				Quantity = quantity,
				Price = new Money() { Amount = price, Currency = Currency.USD },
			};
		}

		[Test]
		public void AddOrUpdateCartItem_ShouldInsertAndPersistItem()
		{
			var cartId = Guid.NewGuid();
			var item = CreateCartItem(1);

			var updatedCart = _service.AddOrUpdateCartItem(cartId, item);

			Assert.That(updatedCart.Items.Count, Is.EqualTo(1));

			var fromDb = _repository.GetById(cartId);
			Assert.That(fromDb, Is.Not.Null);
			Assert.That(fromDb!.Items.First().Id, Is.EqualTo(1));
		}

		[Test]
		public void Get_ShouldReturnNewCart_WhenCartDoesNotExist()
		{
			var cartId = Guid.NewGuid();
			var cart = _service.Get(cartId);

			Assert.That(cart.Id, Is.EqualTo(cartId));
			Assert.That(cart.Items.Count, Is.EqualTo(0));
		}

		[Test]
		public void UpdateCartItemQuantity_ShouldIncreaseQuantity()
		{
			var cartId = Guid.NewGuid();
			var item = CreateCartItem(2, quantity: 1);
			_service.AddOrUpdateCartItem(cartId, item);

			_service.UpdateCartItemQuantity(cartId, item.Id, 2);
			var result = _repository.GetById(cartId);

			Assert.That(result!.Items.First().Quantity, Is.EqualTo(3));
		}

		[Test]
		public void UpdateCartItemQuantity_ShouldRemoveItem_WhenQuantityGoesToZero()
		{
			var cartId = Guid.NewGuid();
			var item = CreateCartItem(3, quantity: 1);
			_service.AddOrUpdateCartItem(cartId, item);

			_service.UpdateCartItemQuantity(cartId, item.Id, -1);
			var result = _repository.GetById(cartId);

			Assert.That(result, Is.Null);
		}
	}
}