using CartService.Domain.Entities;
using Common.Domain.Entities;
using Common.Domain.Enums;
using Common.Domain.ValueObjects;

namespace CartService.Tests.Domain.Entities
{
	[TestFixture]
	public class CartItemTests
	{
		[Test]
		public void Should_Create_CartItem_With_Valid_Data()
		{
			var money = new Money(10, Currency.EUR);
			var cartItem = new CartItem
			{
				Id = 1,
				Name = "Test Item",
				Price = money,
				Quantity = 2
			};

			Assert.That(cartItem.Id, Is.EqualTo(1));
			Assert.That(cartItem.Name, Is.EqualTo("Test Item"));
			Assert.That(cartItem.Quantity, Is.EqualTo(2));
			Assert.That(cartItem.Price, Is.EqualTo(money));
			Assert.That(cartItem.Price.ToString(), Is.EqualTo("10.00 EUR"));
		}

		[Test]
		public void Should_Throw_If_Name_Is_Empty()
		{
			var cartItem = new CartItem
			{
				Id = 2,
				Price = new Money(5, Currency.USD),
				Quantity = 1,
				Name = "Name"
			};

			var ex = Assert.Throws<ArgumentException>(() => cartItem.Name = "");
			StringAssert.Contains("Name", ex.Message);
		}

		[Test]
		public void Should_Throw_If_Quantity_Is_Negative()
		{
			var cartItem = new CartItem
			{
				Id = 3,
				Name = "Another Item",
				Price = new Money(20, Currency.USD)
			};

			var ex = Assert.Throws<ArgumentException>(() => cartItem.Quantity = -1);
			StringAssert.Contains("Quantity", ex.Message);
		}

		[Test]
		public void Should_Allow_Updating_Name_And_Quantity()
		{
			var cartItem = new CartItem
			{
				Id = 4,
				Name = "Old Name",
				Price = new Money(15, Currency.USD),
				Quantity = 3
			};

			cartItem.Name = "New Name";
			cartItem.Quantity = 5;

			Assert.That(cartItem.Name, Is.EqualTo("New Name"));
			Assert.That(cartItem.Quantity, Is.EqualTo(5));
		}

		[Test]
		public void Should_Set_And_Get_Image()
		{
			var image = new Image
			{
				Url = new Uri("https://example.com/image.jpg"),
				AltText = "Sample image"
			};

			var cartItem = new CartItem
			{
				Id = 5,
				Name = "Item with Image",
				Price = new Money(30, Currency.EUR),
				Quantity = 1,
				Image = image
			};

			Assert.NotNull(cartItem.Image);
			Assert.That(cartItem.Image!.Url!.ToString(), Is.EqualTo("https://example.com/image.jpg"));
			Assert.That(cartItem.Image.AltText, Is.EqualTo("Sample image"));
		}

		[Test]
		public void Money_Equals_Should_Return_True_For_Same_Values()
		{
			var m1 = new Money(9.99, Currency.EUR);
			var m2 = new Money(9.99, Currency.EUR);

			Assert.IsTrue(m1.Equals(m2));
		}

		[Test]
		public void Money_Equals_Should_Return_False_For_Different_Values()
		{
			var m1 = new Money(9.99, Currency.EUR);
			var m2 = new Money(9.99, Currency.USD);
			var m3 = new Money(10.00, Currency.EUR);

			Assert.IsFalse(m1.Equals(m2));
			Assert.IsFalse(m1.Equals(m3));
		}

		[Test]
		public void Money_ToString_Should_Format_Correctly()
		{
			var money = new Money(42.5, Currency.USD);
			Assert.That(money.ToString(), Is.EqualTo("42.50 USD"));
		}
	}
}
