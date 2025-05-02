namespace CartService.Domain.Entities
{
	public class Cart
	{
		public required Guid Id { get; set; }

		public List<CartItem> Items { get; private set; } = [];

		public void AddOrUpdateItem(CartItem cartItem)
		{
			var existingIndex = Items.FindIndex(i => i.Id == cartItem.Id);

			if (existingIndex != -1)
			{
				Items[existingIndex] = cartItem;
			}
			else
			{
				Items.Add(cartItem);
			}
		}

		public int UpdateItemQuantity(int id, int quantity)
		{
			var item = Items.FirstOrDefault(i => i.Id == id);

			if (item == null) return 0;

			if (item.Quantity + quantity > 0)
			{
				item.Quantity += quantity;
				return item.Quantity;
			}
			else
			{
				Items.Remove(item);
				return 0;
			}
		}

		public bool DeleteItem(int id)
		{
			var item = Items.FirstOrDefault(i => i.Id == id);

			if (item != null)
			{
				Items.Remove(item);
				return true;
			}
			return false;
		}
	}
}
