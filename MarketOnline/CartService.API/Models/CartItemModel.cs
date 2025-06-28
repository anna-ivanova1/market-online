namespace CartService.API.Models
{
	public class CartItemModel
	{
		public required int Id { get; set; }

		public required string Name { get; set; }

		public ImageModel? Image { get; set; }

		public required MoneyModel Price { get; set; }

		public int Quantity { get; set; }
	}
}
