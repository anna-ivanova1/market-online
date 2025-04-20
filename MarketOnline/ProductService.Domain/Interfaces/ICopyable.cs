namespace CatalogService.Domain.Interfaces
{
	public interface ICopyable<T>
	{
		void CopyTo(T target);
	}
}
