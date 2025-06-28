using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Messages
{
	public class ProductUpdateMessage
	{
		public int Id { get; set; }

		public required string Name { get; set; }

		public required Money Price { get; set; }
	}
}
