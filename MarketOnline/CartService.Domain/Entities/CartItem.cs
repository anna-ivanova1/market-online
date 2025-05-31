using Common.Domain.Entities;

namespace CartService.Domain.Entities
{
	public class CartItem
	{
		private int _quantity = 0;
		private string _name = string.Empty;

		public required int Id { get; init; }

		public required string Name
		{
			get => _name;
			set
			{
				if (string.IsNullOrEmpty(value)) throw new ArgumentException(Properties.Resource.CartItem_Name_MustBeNotEmpty);

				_name = value;
			}
		}

		public Image? Image { get; set; }

		public required Money Price { get; set; }

		public int Quantity
		{
			get => _quantity;
			set
			{
				if (value < 0) throw new ArgumentException(Properties.Resource.CartItem_Quantity_MustBeGreaterZero);

				_quantity = value;
			}
		}
	}
}
