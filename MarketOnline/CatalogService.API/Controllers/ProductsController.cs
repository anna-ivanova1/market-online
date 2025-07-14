using AutoMapper;

using CatalogService.API.Interfaces;
using CatalogService.API.Models;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{
	[Route("api/catalog-service/[controller]")]
	[ApiController]
	[Authorize]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _service;
		private readonly IMapper _mapper;
		private readonly IProductLinkBuilder _productLinkBuilder;

		public ProductsController(IProductService productService, IMapper mapper, IProductLinkBuilder productLinkBuilder)
		{
			_service = productService;
			_mapper = mapper;
			_productLinkBuilder = productLinkBuilder;
		}

		[HttpGet]
		public ActionResult<IEnumerable<ProductModel>> Get(
			[FromQuery] Guid categoryId,
			int pageNumber = 1,
			int pageSize = 10)
		{
			var result = _service
				.List(categoryId, pageNumber, pageSize)
				.Select(GetModel);

			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductModel>> GetById(int id)
		{
			var result = await _service.Get(id);
			return Ok(GetModel(result));
		}


		[HttpPost]
		[Authorize(Roles = "market-online-manager")]
		public async Task<ActionResult<ProductModel>> Add([FromBody] ProductModel model)
		{
			var result = await _service.Add(_mapper.Map<Product>(model));
			return Ok(GetModel(result));
		}

		[HttpPut]
		[Authorize(Roles = "market-online-manager")]
		public async Task<ActionResult<ProductModel>> Update([FromBody] ProductModel model)
		{
			var result = await _service.Update(_mapper.Map<Product>(model));

			return Ok(GetModel(result));
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "market-online-manager")]
		public async Task<ActionResult> Delete(int id)
		{
			var result = await _service.Delete(id);
			return result ? Ok() : NotFound();
		}

		private ProductModel? GetModel(Product? product)
		{
			if (product == null)
			{
				return null;
			}

			var model = _mapper.Map<ProductModel>(product);
			_productLinkBuilder.AssignProductLinks(model);
			return model;
		}
	}
}
