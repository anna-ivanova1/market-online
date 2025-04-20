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

		public void UpdateItemQuantity(int id, int quantity)
		{
			var item = Items.FirstOrDefault(i => i.Id == id);

			if (item == null) return;

			if (item.Quantity + quantity > 0)
			{
				item.Quantity += quantity;
			}
			else
			{
				Items.Remove(item);
			}
		}

		public void DeleteItem(int id)
		{
			var item = Items.FirstOrDefault(i => i.Id == id);

			if (item != null)
			{
				Items.Remove(item);
			}
		}

	}
}
