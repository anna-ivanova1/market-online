using Common.Domain.Interfaces;
using LiteDB;

namespace CartService.Infrastructure.Data
{
	public class PersistentStorage<TKey, TEntity> : IPersistentStorage<TKey, TEntity> where TEntity : class, IEntity<TKey>
	{
		private readonly string _storageName;
		private readonly string _collectionName;

		public PersistentStorage(string storageName, string collectionName)
		{
			_storageName = storageName;
			_collectionName = collectionName;
		}

		public void Add(TEntity entity)
		{
			using var db = new LiteDatabase(_storageName);
			try
			{
				var transactionCreated = db.BeginTrans();
				var col = db.GetCollection<TEntity>(_collectionName);
				col.Insert(entity);

				db.Commit();
			}
			catch
			{
				db.Rollback();
			}
		}

		public TEntity Get(TKey key)
		{
			using var db = new LiteDatabase(_storageName);

			return db.GetCollection<TEntity>(_collectionName).Query().Where("_id = @0", new BsonValue(key)).SingleOrDefault();
		}

		public IList<TEntity> GetAll()
		{
			using var db = new LiteDatabase(_storageName);
			return db.GetCollection<TEntity>(_collectionName).Query().ToList();
		}

		/// <inheritdoc/>
		public void Delete(TKey key)
		{
			using var db = new LiteDatabase(_storageName);
			try
			{
				var transactionCreated = db.BeginTrans();
				var col = db.GetCollection<TEntity>(_collectionName);
				col.Delete(key as BsonValue);

				db.Commit();
			}
			catch
			{
				db.Rollback();
			}
		}

		public void Update(TEntity entity)
		{
			using var db = new LiteDatabase(_storageName);
			try
			{
				var transactionCreated = db.BeginTrans();
				var col = db.GetCollection<TEntity>(_collectionName);
				col.Update(entity);

				db.Commit();
			}
			catch
			{
				db.Rollback();
			}
		}
	}
}
