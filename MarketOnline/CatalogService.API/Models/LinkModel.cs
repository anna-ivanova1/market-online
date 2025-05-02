namespace CatalogService.API.Models
{
	public class LinkModel
	{
		public required string Rel { get; set; }
		public required string Href { get; set; }
		public required string Method { get; set; }
	}
}
