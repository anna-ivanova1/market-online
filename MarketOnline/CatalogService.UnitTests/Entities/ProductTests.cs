namespace CatalogService.UnitTests.Entities
{
	using System;

	using Common.Domain.Enums;

	using global::CatalogService.Domain.Entities;

	using NUnit.Framework;

	namespace CatalogService.Tests.Domain.Entities
	{
		public class ProductTests
		{
			private Category GetSampleCategory() => new() { Id = Guid.NewGuid(), Name = "Sample Category" };

			private Money GetSamplePrice() => new Money() { Amount = 9.99, Currency = Currency.EUR };

			[Test]
			public void Name_SetValidValue_ShouldSucceed()
			{
				var product = CreateValidProduct();
				product.Name = "Notebook";
				Assert.That(product.Name, Is.EqualTo("Notebook"));
			}

			[Test]
			public void Name_SetEmptyValue_ShouldThrow()
			{
				var product = CreateValidProduct();
				var ex = Assert.Throws<ArgumentException>(() => product.Name = "");
				Assert.NotNull(ex!.Message);
			}

			[Test]
			public void Name_SetTooLongValue_ShouldThrow()
			{
				var product = CreateValidProduct();
				var longName = new string('A', 51);
				var ex = Assert.Throws<ArgumentException>(() => product.Name = longName);
				Assert.NotNull(ex!.Message);
			}

			[Test]
			public void Amount_SetNegative_ShouldThrow()
			{
				var product = CreateValidProduct();
				var ex = Assert.Throws<ArgumentException>(() => product.Amount = -1);
				Assert.NotNull(ex!.Message);
			}

			[Test]
			public void CopyTo_ShouldCopyAllProperties()
			{
				var category = GetSampleCategory();
				var source = new Product
				{
					Id = 1,
					Name = "Laptop",
					Description = "A gaming laptop",
					Image = new Uri("http://example.com/laptop.png"),
					Price = GetSamplePrice(),
					CategoryId = category.Id,
					Category = category,
					Amount = 5
				};

				var target = new Product
				{
					Id = 2, // won't be changed
					Name = "Old",
					Price = GetSamplePrice(),
					CategoryId = category.Id,
					Category = category,
					Amount = 1
				};

				source.CopyTo(target);

				Assert.That(target.Name, Is.EqualTo(source.Name));
				Assert.That(target.Description, Is.EqualTo(source.Description));
				Assert.That(target.Image, Is.EqualTo(source.Image));
				Assert.That(target.Price, Is.EqualTo(source.Price));
				Assert.That(target.Category, Is.EqualTo(source.Category));
				Assert.That(target.Amount, Is.EqualTo(source.Amount));
				Assert.That(target.Id, Is.Not.EqualTo(source.Id)); // init-only property remains unchanged
			}

			[Test]
			public void Equals_EqualObjects_ReturnsTrue()
			{
				var id = 1;
				var category = GetSampleCategory();
				var price = GetSamplePrice();

				var p1 = new Product
				{
					Id = id,
					Name = "Phone",
					Description = "Smartphone",
					Image = new Uri("http://example.com/phone.png"),
					Price = price,
					Category = category,
					CategoryId = category.Id,
					Amount = 10
				};

				var p2 = new Product
				{
					Id = id,
					Name = "Phone",
					Description = "Smartphone",
					Image = new Uri("http://example.com/phone.png"),
					Price = price,
					Category = category,
					CategoryId = category.Id,
					Amount = 10
				};

				Assert.IsTrue(p1.Equals(p2));
				Assert.IsTrue(p1 == p2);
				Assert.IsFalse(p1 != p2);
			}

			[Test]
			public void Equals_DifferentObjects_ReturnsFalse()
			{
				var p1 = CreateValidProduct();
				var p2 = CreateValidProduct();
				p2.Name = "NewName";

				Assert.IsFalse(p1.Equals(p2));
				Assert.IsFalse(p1 == p2);
				Assert.IsTrue(p1 != p2);
			}

			[Test]
			public void GetHashCode_EqualObjects_HaveSameHashCode()
			{
				var id = 42;
				var category = GetSampleCategory();
				var price = GetSamplePrice();

				var p1 = new Product
				{
					Id = id,
					Name = "Item",
					Description = "Desc",
					Image = new Uri("http://example.com/image.png"),
					Price = price,
					Category = category,
					CategoryId = category.Id,
					Amount = 3
				};

				var p2 = new Product
				{
					Id = id,
					Name = "Item",
					Description = "Desc",
					Image = new Uri("http://example.com/image.png"),
					Price = price,
					Category = category,
					CategoryId = category.Id,
					Amount = 3
				};

				Assert.That(p2.GetHashCode(), Is.EqualTo(p1.GetHashCode()));
			}

			private Product CreateValidProduct()
			{
				var category = GetSampleCategory();
				return new Product
				{
					Id = 1,
					Name = "Product",
					Description = "Test",
					Price = GetSamplePrice(),
					Category = category,
					CategoryId = category.Id,
					Amount = 1
				};
			}
		}
	}

}
