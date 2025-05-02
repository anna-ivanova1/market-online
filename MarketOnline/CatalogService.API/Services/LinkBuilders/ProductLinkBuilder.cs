using CatalogService.API.Interfaces;
using CatalogService.API.Models;

namespace CatalogService.API.Services.LinkBuilders
{
	public class ProductLinkBuilder : IProductLinkBuilder
	{
		private readonly LinkGenerator _linkGenerator;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ProductLinkBuilder(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
		{
			_linkGenerator = linkGenerator;
			_httpContextAccessor = httpContextAccessor;
		}


		public void AssignProductLinks(ProductModel model)
		{
			var httpContext = _httpContextAccessor.HttpContext!;

			var links = new List<LinkModel>
			{
				new()
				{
					Rel = "self",
					Href = _linkGenerator
					.GetPathByAction(httpContext, "GetById", "Products", new { id = model.Id })!,
					Method = "GET"
				},
				new()
				{
					Rel = "delete",
					Href = _linkGenerator.GetPathByAction(httpContext, "Delete", "Products", new { id = model.Id })!,
					Method = "DELETE"
				},
				new()
				{
					Rel = "getCategory",
					Href = _linkGenerator.GetPathByAction(httpContext, "GetById", "Categories", new { id = model.CategoryId })!,
					Method = "GET"
				},
			};
		}
	}
}
