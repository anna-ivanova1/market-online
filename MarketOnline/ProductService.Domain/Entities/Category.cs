using CatalogService.Domain.Interfaces;

namespace CatalogService.Domain.Entities
{
	public sealed class Category : ICopyable<Category>, IEquatable<Category>
	{
		private const int MAX_LENGTH_NAME = 50;

		private string _name = string.Empty;

		public Guid Id { get; set; }

		public required string Name
		{
			get => _name;
			set
			{
				if (string.IsNullOrEmpty(value)) throw new ArgumentException(Properties.Resource.Category_Name_MustBeNotEmpty);
				if (value.Length > MAX_LENGTH_NAME) throw new ArgumentException(Properties.Resource.Category_Name_IsTooLong);

				_name = value;
			}
		}

		public Uri? Image { get; set; }

		public Guid? ParentId { get; set; }

		public Category? Parent { get; set; }

		public void CopyTo(Category target)
		{
			if (target == null) return;
			target.Id = Id;
			target.Name = Name;
			target.Image = Image;
			target.Parent = Parent;
		}

		public bool Equals(Category? other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;

			return Id == other.Id &&
				   Name == other.Name &&
				   Equals(Image, other.Image) &&
				   (Parent is null && other.Parent is null ||
					Parent is not null && Parent.Equals(other.Parent));
		}

		public override bool Equals(object? obj) => Equals(obj as Category);

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Name, Image, Parent);
		}

		public static bool operator ==(Category? left, Category? right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Category? left, Category? right)
		{
			return !Equals(left, right);
		}
	}
}
