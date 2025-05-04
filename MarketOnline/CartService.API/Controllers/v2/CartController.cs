using Asp.Versioning;
using AutoMapper;
using CartService.API.Models;
using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Controllers.v2
{
	/// <summary>
	/// Cart service controller version 2.0. Provides endpoints for managing cart items.
	/// </summary>
	[ApiVersion("2.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;
		private readonly IMapper _mapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="CartController"/> class.
		/// </summary>
		/// <param name="cartService">Service for cart operations.</param>
		/// <param name="mapper">Mapper to convert between models and entities.</param>
		public CartController(ICartService cartService, IMapper mapper)
		{
			_cartService = cartService;
			_mapper = mapper;
		}

		/// <summary>
		/// Retrieves all items in the specified cart.
		/// If the cart does not exist, a new one is created and returned.
		/// </summary>
		/// <param name="cartId">The ID of the cart (GUID format expected).</param>
		/// <returns>A list of <see cref="CartItemModel"/> in the cart.</returns>
		/// <response code="200">Returns the list of cart items.</response>
		/// <response code="400">If the cart ID is not a valid GUID.</response>
		/// <exception cref="ArgumentException">Thrown when the cart ID is invalid.</exception>
		[MapToApiVersion("2.0")]
		[HttpGet("{cartId}")]
		[ProducesResponseType(typeof(IEnumerable<CartItemModel>), 200)]
		[ProducesResponseType(400)]
		public ActionResult<IEnumerable<CartItemModel>> Get(string cartId)
		{
			var cartIdGuid = GetGuidId(cartId);

			var cartItems = _cartService.Get(cartIdGuid).Items.Select(_mapper.Map<CartItemModel>);

			return Ok(cartItems);
		}

		/// <summary>
		/// Adds or updates an item in the specified cart.
		/// </summary>
		/// <param name="cartId">The ID of the cart (GUID format expected).</param>
		/// <param name="item">The cart item to add or update.</param>
		/// <returns>HTTP 200 OK on success.</returns>
		/// <response code="200">Item successfully added or updated.</response>
		/// <response code="400">If the cart ID is invalid or item is null.</response>
		/// <exception cref="ArgumentException">Thrown when the cart ID is invalid.</exception>
		[MapToApiVersion("2.0")]
		[HttpPost("{cartId}/items")]
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
		/// Deletes an item from the specified cart.
		/// </summary>
		/// <param name="cartId">The ID of the cart (GUID format expected).</param>
		/// <param name="itemId">The ID of the item to remove.</param>
		/// <returns>HTTP 200 OK if the item was removed; 404 if the item was not found.</returns>
		/// <response code="200">Item successfully deleted.</response>
		/// <response code="404">Item not found in the cart.</response>
		/// <exception cref="ArgumentException">Thrown when the cart ID is invalid.</exception>
		[MapToApiVersion("2.0")]
		[HttpDelete("{cartId}/items/{itemId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public IActionResult DeleteItem(string cartId, int itemId)
		{
			var cartIdGuid = GetGuidId(cartId);
			return _cartService.DeleteCartItem(cartIdGuid, itemId) ? Ok() : NotFound();
		}

		/// <summary>
		/// Parses and validates the provided cart ID string.
		/// </summary>
		/// <param name="id">Cart ID as a string.</param>
		/// <returns>A valid GUID representing the cart ID.</returns>
		/// <exception cref="ArgumentException">Thrown when the ID is not a valid GUID.</exception>
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
