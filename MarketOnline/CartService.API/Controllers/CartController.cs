using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CartController : ControllerBase
	{
		private readonly ILogger<CartController> _logger;
		private readonly ICartService _cartService;

		public CartController(ICartService cartService, ILogger<CartController> logger)
		{
			_logger = logger;
			_cartService = cartService;
		}

		[HttpGet(Name = "GetById")]
		public Cart Get(Guid id)
		{
			return _cartService.Get(id);
		}
	}
}
