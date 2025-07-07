namespace CartService.API.Models
{
	/// <summary>
	/// Cart model
	/// </summary>
	public class CartModel
	{

		/// <summary>
		/// Gets or sets Id
		/// </summary>
		public required Guid Id { get; set; }


		/// <summary>
		/// Gets Items
		/// </summary>
		public List<CartItemModel> Items { get; private set; } = [];
	}
}
