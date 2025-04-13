using CartService.Domain.Entities;

namespace CartService.Domain.Tests.Entities
{
	public class CartTests
	{
		[Test]
		public void Test_AddItem_NewItem_ShouldAddToCart()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var cartItem = new CartItem
			{
				Id = 1,
			};

			cart.AddItem(cartItem);

			Assert.That(cart.Items.Count, Is.EqualTo(1));
			Assert.That(cart.Items.First(), Is.EqualTo(cartItem));
		}

		[Test]
		public void Test_AddItem_ExistingItem_ShouldIncreaseQuantity()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var cartItem = new CartItem
			{
				Id = 1,
				Quantity = 1
			};

			cart.AddItem(cartItem);
			cart.AddItem(cartItem);

			Assert.That(cart.Items.Count, Is.EqualTo(1));
			Assert.That(cart.Items.First().Quantity, Is.EqualTo(2));
		}

		[Test]
		public void Test_RemoveItem_ExistingItem_ShouldRemoveItem()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var cartItem = new CartItem
			{
				Id = 1,
				Quantity = 1
			};

			cart.AddItem(cartItem);
			cart.RemoveItem(1);

			Assert.That(cart.Items.Count, Is.EqualTo(0));
		}

		[Test]
		public void Test_RemoveItem_ExistingItemWithQuantityGreaterThanOne_ShouldDecreaseQuantity()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var cartItem = new CartItem
			{
				Id = 1,
				Quantity = 2
			};

			cart.AddItem(cartItem);
			cart.RemoveItem(1);

			Assert.That(cart.Items.Count, Is.EqualTo(1));
			Assert.That(cart.Items.First().Quantity, Is.EqualTo(1));
		}

		[Test]
		public void Test_RemoveItem_NonExistingItem_ShouldDoNothing()
		{
			var cart = new Cart { Id = Guid.NewGuid() };
			var cartItem = new CartItem
			{
				Id = 1,
				Quantity = 1
			};

			cart.AddItem(cartItem);
			cart.RemoveItem(999);

			Assert.That(cart.Items.Count, Is.EqualTo(1));
			Assert.That(cart.Items.First().Quantity, Is.EqualTo(1));
		}
	}
}
