namespace CatalogService.UnitTests.Entities
{
	using global::CatalogService.Domain.Entities;
	using NUnit.Framework;
	using System;

	namespace CatalogService.Tests.Domain.Entities
	{
		public class CategoryTests
		{
			[Test]
			public void Name_SetValidValue_ShouldSetSuccessfully()
			{
				var category = new Category { Name = "Electronics" };
				Assert.AreEqual("Electronics", category.Name);
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

				Assert.AreEqual(source.Id, target.Id);
				Assert.AreEqual(source.Name, target.Name);
				Assert.AreEqual(source.Image, target.Image);
				Assert.AreEqual(source.Parent, target.Parent);
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

				Assert.AreEqual(category1.GetHashCode(), category2.GetHashCode());
			}
		}
	}

}
