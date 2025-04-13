namespace CartService.Application.Entities
{
	public class CartDto
	{
		public required Guid Id { get; set; }

		public required IReadOnlyCollection<CartItemDto> Items { get; set; }
	}
}
