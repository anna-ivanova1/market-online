namespace Common.Domain.Interfaces
{
	public interface IEntity<TKey>
	{
		/// <summary>
		/// Identifier
		/// </summary>
		TKey Id { get; set; }
	}
}
