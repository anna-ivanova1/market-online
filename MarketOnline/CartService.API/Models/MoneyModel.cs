namespace CartService.API.Models
{
	public class MoneyModel
	{
		public double Amount { get; set; }

		public required string Currency { get; set; }
	}
}
