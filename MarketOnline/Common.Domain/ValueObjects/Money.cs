using Common.Domain.Enums;

namespace Common.Domain.ValueObjects
{
	public struct Money(double amount, Currency currency) : IEquatable<Money>
	{
		public double Amount { get; } = amount;

		public Currency Currency { get; } = currency;

		public override readonly string ToString() => $"{Amount:0.00} {Currency}";

		public readonly bool Equals(Money other) => Amount == other.Amount && Currency == other.Currency;

		public override readonly int GetHashCode() => HashCode.Combine(Amount, Currency);
	}
}
