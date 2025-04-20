namespace CatalogService.Domain.Interfaces
{
	public interface IRepository<T, K> where T : class
	{
		public Task<T> Get(K id);

		public IEnumerable<T> List();

		public Task<K> Add(T item);

		public Task Update(T item);

		public Task Delete(K id);
	}
}
