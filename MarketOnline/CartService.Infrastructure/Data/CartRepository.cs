using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using LiteDB;

namespace CartService.Infrastructure.Data
{
	public class CartRepository : ICartRepository
	{
		private readonly string _databasePath;

		public CartRepository(string databasePath = "C:/temp/CartData.db")
		{
			_databasePath = databasePath;
		}

		public void Upsert(Cart cart)
		{
			using var db = new LiteDatabase(_databasePath);
			var collection = db.GetCollection<Cart>("carts");
			collection.Upsert(cart);
			var item = collection.FindById(cart.Id);
		}

		public Cart? GetById(Guid id)
		{
			using var db = new LiteDatabase(_databasePath);
			var collection = db.GetCollection<Cart>("carts");
			return collection.FindById(id);
		}

		public bool Delete(Guid id)
		{
			using var db = new LiteDatabase(_databasePath);
			var collection = db.GetCollection<Cart>("carts");
			return collection.Delete(id);
		}
	}
}
