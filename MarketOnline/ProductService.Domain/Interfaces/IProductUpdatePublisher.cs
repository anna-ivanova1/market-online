using CatalogService.Domain.Messages;

namespace CatalogService.Domain.Interfaces
{
	public interface IProductUpdatePublisher
	{
		Task PublishProductUpdate(ProductUpdateMessage message);
	}
}
