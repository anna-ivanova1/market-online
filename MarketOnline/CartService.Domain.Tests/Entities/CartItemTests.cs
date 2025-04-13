using CartService.Domain.Entities;

namespace CartService.Domain.Tests.Entities
{
	public class CartItemTests
	{
		[Test]
		public void Test_SetQuantity_ValidQuantity_ShouldSetQuantity()
		{
			var cartItem = new CartItem
			{
				Id = 1,
			};

			cartItem.Quantity = 5;
			Assert.That(cartItem.Quantity, Is.EqualTo(5));
		}

		[Test]
		public void Test_SetQuantity_NegativeQuantity_ShouldThrowArgumentException()
		{
			var cartItem = new CartItem
			{
				Id = 1,
			};
			var ex = Assert.Throws<ArgumentException>(() => cartItem.Quantity = -1);
			Assert.That(ex.Message, Is.EqualTo(Properties.Resource.CartItem_Quantity_MustBeGreaterZero));
		}
	}
}
