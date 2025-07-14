using Common.Domain.Enums;

namespace CatalogService.Domain.Entities
{
	public sealed class Money : IEquatable<Money>
	{
		public double Amount { get; set; }

		public Currency Currency { get; set; }

		public override string ToString() => $"{Amount:0.00} {Currency}";

		public override bool Equals(object? obj)
		{
			if (obj == null) return false;
			if (!(obj is Money moneyObj)) return false;


			return moneyObj.Amount == Amount && moneyObj.Currency == Currency;
		}

		public bool Equals(Money? other)
		{
			return other != null && Amount == other.Amount && Currency == other.Currency;
		}

		public override int GetHashCode() => HashCode.Combine(Amount, Currency);
	}
}
