using Common.Domain.Entities;
using Common.Domain.ValueObjects;

namespace CartService.Application.Entities
{
	public class CartItemDto
	{
		public required int Id { get; init; }

		public required string Name { get; set; }

		public required Money Price { get; set; }

		public Image? Image { get; set; }

		public int Quantity { get; set; }
	}
}
