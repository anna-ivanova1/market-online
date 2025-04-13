using Common.Domain.Entities;
using Common.Domain.ValueObjects;

namespace ProductService.Application.Entities
{
	public class ProductShortDto
	{
		public required int Id { get; init; }

		public required string Name { get; set; }

		public Image? Image { get; set; }

		public required Money Price { get; set; }
	}
}
