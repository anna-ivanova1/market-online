namespace CatalogService.API.Models
{
	public class CategoryModel
	{
		public Guid Id { get; set; }

		public required string Name { get; set; }

		public Uri? Image { get; set; }

		public Guid? ParentId { get; set; }
	}
}
