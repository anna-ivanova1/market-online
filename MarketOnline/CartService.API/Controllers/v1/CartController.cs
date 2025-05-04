using Asp.Versioning;
using AutoMapper;
using CartService.API.Models;
using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Controllers.v1
{
	/// <summary>
	/// Cart service controller version 1.0. Handles operations for retrieving, adding, and removing cart items.
	/// </summary>
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;
		private readonly IMapper _mapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="CartController"/> class.
		/// </summary>
		/// <param name="cartService">Service handling cart logic.</param>
		/// <param name="mapper">Object mapper for converting between models and entities.</param>
		public CartController(ICartService cartService, IMapper mapper)
		{
			_cartService = cartService;
			_mapper = mapper;
		}

		/// <summary>
		/// Retrieves a cart by its ID. Creates a new cart if none exists with the provided ID.
		/// </summary>
		/// <param name="cartId">Cart ID in GUID format.</param>
		/// <returns>The requested or newly created <see cref="CartModel"/>.</returns>
		/// <response code="200">Returns the cart successfully.</response>
		/// <response code="400">If the cart ID is not a valid GUID.</response>
		/// <exception cref="ArgumentException">Thrown when the cart ID cannot be parsed as a GUID.</exception>
		[MapToApiVersion("1.0")]
		[HttpGet("{key}")]
		[ProducesResponseType(typeof(CartModel), 200)]
		[ProducesResponseType(400)]
		public ActionResult<CartModel> Get(string cartId)
		{
			var cartIdGuid = GetGuidId(cartId);
			var cart = _cartService.Get(cartIdGuid);
			return Ok(_mapper.Map<CartModel>(cart));
		}

		/// <summary>
		/// Adds a new item to the specified cart or updates it if it already exists.
		/// </summary>
		/// <param name="cartId">Cart ID in GUID format.</param>
		/// <param name="item">The item to add or update.</param>
		/// <returns>HTTP 200 OK if successful.</returns>
		/// <response code="200">Item successfully added or updated.</response>
		/// <response code="400">If the cart ID is invalid or item is null.</response>
		/// <exception cref="ArgumentException">Thrown when the cart ID is invalid.</exception>
		[MapToApiVersion("1.0")]
		[HttpPost("{key}/items")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public IActionResult AddItem(string cartId, [FromBody] CartItemModel item)
		{
			var itemToAdd = _mapper.Map<CartItem>(item);
			var cartIdGuid = GetGuidId(cartId);
			_cartService.AddOrUpdateCartItem(cartIdGuid, itemToAdd);
			return Ok();
		}

		/// <summary>
		/// Removes an item from the specified cart.
		/// </summary>
		/// <param name="cartId">Cart ID in GUID format.</param>
		/// <param name="itemId">The identifier of the item to remove.</param>
		/// <returns>HTTP 200 OK if the item was removed; 404 Not Found if not found.</returns>
		/// <response code="200">Item successfully deleted.</response>
		/// <response code="404">Item or cart not found.</response>
		/// <exception cref="ArgumentException">Thrown when the cart ID is invalid.</exception>
		[MapToApiVersion("1.0")]
		[HttpDelete("{key}/items/{itemId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public IActionResult DeleteItem(string cartId, int itemId)
		{
			var cartIdGuid = GetGuidId(cartId);
			return _cartService.DeleteCartItem(cartIdGuid, itemId) ? Ok() : NotFound();
		}

		private Guid GetGuidId(string id)
		{
			if (!Guid.TryParse(id, out Guid cartIdGuid))
			{
				throw new ArgumentException("Cart ID is invalid.");
			}

			return cartIdGuid;
		}
	}
}
