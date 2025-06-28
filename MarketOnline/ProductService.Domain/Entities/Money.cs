using Common.Domain.Enums;

namespace CatalogService.Domain.Entities
{
	public class Money : IEquatable<Money>
	{
		public double Amount { get; set; }

		public Currency Currency { get; set; }

		public override string ToString() => $"{Amount:0.00} {Currency}";

		public bool Equals(Money? other)
		{
			return other != null && Amount == other.Amount && Currency == other.Currency;
		}

		public override int GetHashCode() => HashCode.Combine(Amount, Currency);
	}
}
