using AutoMapper;
using CartService.API.Models;
using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cart")]
public class CartV1Controller : ControllerBase
{
	private readonly ICartService _cartService;
	private readonly IMapper _mapper;

	public CartV1Controller(ICartService cartService, Mapper mapper)
	{
		_cartService = cartService;
		_mapper = mapper;
	}

	[HttpGet("{key}")]
	public ActionResult<CartModel> Get(Guid key)
	{
		return Ok(_mapper.Map<CartModel>(_cartService.Get(key)));
	}

	[HttpPost("{key}/items")]
	public IActionResult Add(Guid key, [FromBody] CartItemModel item)
	{
		var itemToAdd = _mapper.Map<CartItem>(item);
		_cartService.AddOrUpdateCartItem(key, itemToAdd);
		return Ok();
	}

	[HttpDelete("{key}/items/{itemId}")]
	public IActionResult DeleteItem(Guid key, int itemId)
	{
		return _cartService.DeleteCartItem(key, itemId) ? Ok() : NotFound();
	}
}
