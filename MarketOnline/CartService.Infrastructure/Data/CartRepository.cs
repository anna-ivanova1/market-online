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

		public IEnumerable<Cart> List()
		{
			using var db = new LiteDatabase(_databasePath);
			var collection = db.GetCollection<Cart>("carts");
			return collection.FindAll().ToList();
		}

		public void Upsert(Cart cart)
		{
			using var db = new LiteDatabase(_databasePath);
			var collection = db.GetCollection<Cart>("carts");
			collection.Upsert(cart);
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

		public void UpdateCartItems(int id, string name, Money price)
		{
			using var db = new LiteDatabase(_databasePath);
			var collection = db.GetCollection<Cart>("carts");
			var cartsToUpdate = collection.Find(_ => _.Items.Select(item => item.Id).Any(itemId => itemId == id));

			foreach (var cart in cartsToUpdate)
			{
				cart.Items.ForEach(item =>
				{
					if (item.Id == id)
					{
						item.Price = price;
						item.Name = name;

						collection.Upsert(cart);
					}
				});
			}
		}
	}
}
