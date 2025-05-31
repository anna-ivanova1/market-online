using CatalogService.Domain.Interfaces;

namespace CatalogService.Domain.Entities
{
	public class Product : ICopyable<Product>, IEquatable<Product>
	{
		private const int MAX_LENGTH_NAME = 50;

		private string _name = string.Empty;
		private int _amount = 0;

		public required string Name
		{
			get => _name;
			set
			{
				if (string.IsNullOrEmpty(value)) throw new ArgumentException(Properties.Resource.Product_Name_MustBeNotEmpty);
				if (value.Length > MAX_LENGTH_NAME) throw new ArgumentException(Properties.Resource.Product_Name_IsTooLong);

				_name = value;
			}
		}

		public string Description { get; set; } = string.Empty;

		public Uri? Image { get; set; }

		public int Id { get; init; }

		public required Money Price { get; set; }

		public required Guid CategoryId { get; set; }

		public required Category Category { get; set; }

		public required int Amount
		{
			get => _amount; set
			{
				if (value < 0) throw new ArgumentException(Properties.Resource.Product_Amount_MustBeEqualOrMoreThenZero);

				_amount = value;
			}
		}

		public void CopyTo(Product target)
		{
			target.Name = Name;
			target.Description = Description;
			target.Image = Image;
			target.Price = Price;
			target.Category = Category;
			target.Amount = Amount;
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as Product);
		}

		public bool Equals(Product? other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;

			return Id == other.Id &&
				   Name == other.Name &&
				   Description == other.Description &&
				   Equals(Image, other.Image) &&
				   Amount == other.Amount &&
				   Equals(Price, other.Price) &&
				   Equals(Category, other.Category);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Name, Description, Image, Amount, Price, Category);
		}

		public static bool operator ==(Product? left, Product? right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Product? left, Product? right)
		{
			return !Equals(left, right);
		}
	}
}
