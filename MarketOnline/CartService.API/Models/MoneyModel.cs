namespace CartService.API.Models
{
	/// <summary>
	/// Money model
	/// </summary>
	public class MoneyModel
	{
		/// <summary>
		/// Gets or sets Amount
		/// </summary>
		public double Amount { get; set; }

		/// <summary>
		/// Gets or sets Currency
		/// </summary>
		public required string Currency { get; set; }
	}
}
