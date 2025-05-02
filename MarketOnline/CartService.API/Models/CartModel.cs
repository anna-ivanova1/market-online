namespace CartService.API.Models
{
	public class CartModel
	{
		public required Guid Id { get; set; }

		public List<CartItemModel> Items { get; private set; } = [];
	}
}
