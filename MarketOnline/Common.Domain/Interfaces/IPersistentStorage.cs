namespace Common.Domain.Interfaces
{
	public interface IPersistentStorage<TKey, TEntity> where TEntity : class, IEntity<TKey>
	{
		void Add(TEntity entity);

		void Update(TEntity entity);

		TEntity Get(TKey key);

		IList<TEntity> GetAll();

		void Delete(TKey key);
	}
}
