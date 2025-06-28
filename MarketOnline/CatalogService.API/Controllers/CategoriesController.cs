using AutoMapper;
using CatalogService.API.Models;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{
	[Route("api/catalog-service/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryService _service;
		private readonly IMapper _mapper;

		public CategoriesController(ICategoryService service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}

		[HttpGet]
		public ActionResult<IEnumerable<CategoryModel>> Get()
		{
			return Ok(_service.List().Select(_mapper.Map<CategoryModel>));
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<CategoryModel>> GetById(Guid id)
		{
			var result = await _service.Get(id);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<CategoryModel>(result));
		}

		[HttpPost]
		public async Task<ActionResult<CategoryModel>> Add([FromBody] CategoryModel model)
		{
			var modelToSave = _mapper.Map<Category>(model);
			if (model.ParentId.HasValue)
			{
				modelToSave.Parent = await _service.Get(model.ParentId.Value);
			}
			var result = await _service.Add(modelToSave);
			return Ok(_mapper.Map<CategoryModel>(result));
		}

		[HttpPut]
		public async Task<ActionResult<CategoryModel>> Update([FromBody] CategoryModel model)
		{
			var modelToUpdate = _mapper.Map<Category>(model);

			if (model.ParentId.HasValue)
			{
				modelToUpdate.Parent = await _service.Get(model.ParentId.Value);
			}
			var result = await _service.Update(modelToUpdate);
			return Ok(_mapper.Map<CategoryModel>(result));
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(Guid id)
		{
			var result = await _service.Delete(id);
			return result ? Ok() : NotFound();
		}
	}
}
