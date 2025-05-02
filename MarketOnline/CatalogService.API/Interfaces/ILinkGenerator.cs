using CatalogService.API.Models;

namespace CatalogService.API.Interfaces
{
	public interface IProductLinkBuilder
	{
		void AssignProductLinks(ProductModel model);
	}
}
