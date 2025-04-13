using Common.Domain.Entities;
using Common.Domain.ValueObjects;

namespace ProductService.Domain.Entities
{
	public class Product
	{
		private const int MAX_LENGTH_NAME = 50;

		private string _name = string.Empty;

		public required int Id { get; init; }

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

		public Image? Image { get; set; }

		public required Money Price { get; set; }
	}
}
