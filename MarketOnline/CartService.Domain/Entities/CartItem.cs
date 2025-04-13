namespace CartService.Domain.Entities
{
	public class CartItem
	{
		private int _quantity = 0;

		public required int Id { get; init; }

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
