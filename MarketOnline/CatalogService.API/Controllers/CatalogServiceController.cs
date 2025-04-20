using CatalogService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CatalogServiceController : ControllerBase
	{

		private readonly ILogger<CatalogServiceController> _logger;
		private readonly IProductService _productService;

		public CatalogServiceController(IProductService productService, ILogger<CatalogServiceController> logger)
		{
			_logger = logger;
			_productService = productService;
		}

	}
}
