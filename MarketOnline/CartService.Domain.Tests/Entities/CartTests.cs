using CartService.Domain.Entities;
using Common.Domain.Enums;
using Common.Domain.ValueObjects;

namespace CartService.Tests.Domain.Entities
{
	[TestFixture]
	public class CartTests
	{
		private static CartItem CreateCartItem(int id, string name = "Item", double price = 10, int quantity = 1)
		{
			return new CartItem
			{
				Id = id,
				Name = name,
				Price = new Money(price, Currency.USD),
				Quantity = quantity
			};
		}

		[Test]
		public void AddOrUpdateItem_ShouldAddNewItem_WhenItemNotExists()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var item = CreateCartItem(1);

			cart.AddOrUpdateItem(item);

			Assert.That(cart.Items.Count, Is.EqualTo(1));
			Assert.That(cart.Items.First().Id, Is.EqualTo(1));
		}

		[Test]
		public void AddOrUpdateItem_ShouldUpdateItem_WhenItemExists()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var original = CreateCartItem(1, "Old", 5, 2);
			var updated = CreateCartItem(1, "Updated", 20, 4);

			cart.AddOrUpdateItem(original);
			cart.AddOrUpdateItem(updated);

			var item = cart.Items.First();
			Assert.That(cart.Items.Count, Is.EqualTo(1));
			Assert.That(item.Name, Is.EqualTo("Updated"));
			Assert.That(item.Price.Amount, Is.EqualTo(20));
			Assert.That(item.Quantity, Is.EqualTo(4));
		}

		[Test]
		public void UpdateItemQuantity_ShouldIncreaseQuantity_WhenPositiveValueGiven()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var item = CreateCartItem(1, quantity: 2);
			cart.AddOrUpdateItem(item);

			cart.UpdateItemQuantity(1, 3);

			var result = cart.Items.First();
			Assert.That(result.Quantity, Is.EqualTo(5));
		}

		[Test]
		public void UpdateItemQuantity_ShouldDecreaseQuantity_WhenNegativeValueGiven()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var item = CreateCartItem(1, quantity: 5);
			cart.AddOrUpdateItem(item);

			cart.UpdateItemQuantity(1, -2);

			Assert.That(cart.Items.First().Quantity, Is.EqualTo(3));
		}

		[Test]
		public void UpdateItemQuantity_ShouldRemoveItem_WhenQuantityBecomesZeroOrLess()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var item = CreateCartItem(1, quantity: 1);
			cart.AddOrUpdateItem(item);

			cart.UpdateItemQuantity(1, -1);

			Assert.That(cart.Items.Count, Is.EqualTo(0));
		}

		[Test]
		public void UpdateItemQuantity_ShouldDoNothing_WhenItemNotFound()
		{
			var cart = new Cart { Id = Guid.NewGuid() };

			cart.UpdateItemQuantity(999, 2);

			Assert.That(cart.Items.Count, Is.EqualTo(0));
		}
	}
}
