using Common.Domain.Interfaces;

namespace CartService.Domain.Entities
{
	public class Cart : IEntity<Guid>
	{
		private readonly List<CartItem> _items = [];

		public required Guid Id { get; set; }

		public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

		public void AddItem(CartItem cartItem)
		{
			var existing = _items.FirstOrDefault(i => i.Id == cartItem.Id);

			if (existing != null)
			{
				existing.Quantity += cartItem.Quantity;
			}
			else
			{
				_items.Add(cartItem);
			}
		}

		public void RemoveItem(int id)
		{
			var item = _items.FirstOrDefault(i => i.Id == id);

			if (item == null) return;

			if (item.Quantity > 1)
			{
				item.Quantity--;
			}
			else
			{
				_items.Remove(item);
			}
		}
	}
}
