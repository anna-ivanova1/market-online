namespace CatalogService.UnitTests.Entities
{
	using System;

	using global::CatalogService.Domain.Entities;

	using NUnit.Framework;

	namespace CatalogService.Tests.Domain.Entities
	{
		public class CategoryTests
		{
			[Test]
			public void Name_SetValidValue_ShouldSetSuccessfully()
			{
				var category = new Category { Name = "Electronics" };
				Assert.That(category.Name, Is.EqualTo("Electronics"));
			}

			[Test]
			public void Name_SetEmptyValue_ShouldThrowArgumentException()
			{
				var category = new Category { Name = "Electronics" };
				var ex = Assert.Throws<ArgumentException>(() => category.Name = "");
				Assert.NotNull(ex!.Message);
			}

			[Test]
			public void Name_SetTooLongValue_ShouldThrowArgumentException()
			{
				var category = new Category { Name = "Electronics" };
				string longName = new string('A', 51);
				var ex = Assert.Throws<ArgumentException>(() => category.Name = longName);
				Assert.NotNull(ex!.Message);
			}

			[Test]
			public void CopyTo_ShouldCopyAllProperties()
			{
				var parentCategory = new Category { Id = Guid.NewGuid(), Name = "Parent" };
				var source = new Category
				{
					Id = Guid.NewGuid(),
					Name = "Books",
					Image = new Uri("http://example.com/image.png"),
					Parent = parentCategory
				};

				var target = new Category { Name = "Temp" };

				source.CopyTo(target);

				Assert.That(target.Id, Is.EqualTo(source.Id));
				Assert.That(target.Name, Is.EqualTo(source.Name));
				Assert.That(target.Image, Is.EqualTo(source.Image));
				Assert.That(target.Parent, Is.EqualTo(source.Parent));
			}

			[Test]
			public void Equals_SameReference_ReturnsTrue()
			{
				var category = new Category { Id = Guid.NewGuid(), Name = "Books" };
				Assert.IsTrue(category.Equals(category));
			}

			[Test]
			public void Equals_EqualObjects_ReturnsTrue()
			{
				var parent = new Category { Id = Guid.NewGuid(), Name = "Parent" };
				var id = Guid.NewGuid();
				var image = new Uri("http://example.com/image.png");

				var category1 = new Category { Id = id, Name = "Books", Image = image, Parent = parent };
				var category2 = new Category { Id = id, Name = "Books", Image = image, Parent = new Category { Id = parent.Id, Name = parent.Name } };

				Assert.IsTrue(category1.Equals(category2));
			}

			[Test]
			public void Equals_DifferentObjects_ReturnsFalse()
			{
				var category1 = new Category { Id = Guid.NewGuid(), Name = "Books" };
				var category2 = new Category { Id = Guid.NewGuid(), Name = "Movies" };

				Assert.IsFalse(category1.Equals(category2));
			}

			[Test]
			public void OperatorEquality_SameValues_ReturnsTrue()
			{
				var id = Guid.NewGuid();
				var category1 = new Category { Id = id, Name = "Books" };
				var category2 = new Category { Id = id, Name = "Books" };

				Assert.IsTrue(category1 == category2);
			}

			[Test]
			public void OperatorInequality_DifferentValues_ReturnsTrue()
			{
				var category1 = new Category { Id = Guid.NewGuid(), Name = "Books" };
				var category2 = new Category { Id = Guid.NewGuid(), Name = "Movies" };

				Assert.IsTrue(category1 != category2);
			}

			[Test]
			public void GetHashCode_ShouldBeConsistentWithEquals()
			{
				var id = Guid.NewGuid();
				var category1 = new Category { Id = id, Name = "Books" };
				var category2 = new Category { Id = id, Name = "Books" };

				Assert.That(category2.GetHashCode(), Is.EqualTo(category1.GetHashCode()));
			}
		}
	}

}
