using Common.Domain.ValueObjects;

namespace CatalogService.API.Models
{
	public class ProductModel
	{
		public required string Name { get; set; }

		public string Description { get; set; } = string.Empty;

		public Uri? Image { get; set; }

		public int Id { get; init; }

		public required Money Price { get; set; }

		public required Guid CategoryId { get; set; }

		public required int Amount { get; set; }

		public IEnumerable<LinkModel>? Links { get; set; }
	}
}
