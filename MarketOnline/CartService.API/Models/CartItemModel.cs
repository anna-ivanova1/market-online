namespace CartService.API.Models
{
	/// <summary>
	/// CartItemModel
	/// </summary>
	public class CartItemModel
	{
		/// <summary>
		/// Gets or sets Id
		/// </summary>
		public required int Id { get; set; }

		/// <summary>
		/// Gets or sets Name
		/// </summary>
		public required string Name { get; set; }

		/// <summary>
		/// Gets or sets Image
		/// </summary>
		public ImageModel? Image { get; set; }

		/// <summary>
		/// Gets or sets Price
		/// </summary>
		public required MoneyModel Price { get; set; }

		/// <summary>
		/// Gets or sets Quantity
		/// </summary>
		public int Quantity { get; set; }
	}
}
