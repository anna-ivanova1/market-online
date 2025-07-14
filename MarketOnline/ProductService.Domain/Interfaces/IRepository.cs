namespace CatalogService.Domain.Interfaces
{
	public interface IRepository<T, K> where T : class
	{
		public Task<T?> Get(K id);

		public IQueryable<T> List();

		public Task<K> Add(T item);

		public Task Update(T item);

		public Task<bool> Delete(K id);
	}
}
