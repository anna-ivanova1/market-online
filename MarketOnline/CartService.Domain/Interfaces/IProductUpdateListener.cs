namespace CartService.Domain.Interfaces
{
	public interface IProductUpdateListener
	{
		Task Start();
		Task Stop();
	}
}
