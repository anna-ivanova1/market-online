namespace CatalogService.Domain.Interfaces
{
	public interface ICopyable<in T>
	{
		void CopyTo(T target);
	}
}
