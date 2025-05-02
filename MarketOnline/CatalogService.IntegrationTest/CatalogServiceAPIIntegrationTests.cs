using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace CatalogService.IntegrationTest
{
	internal class CatalogServiceAPIIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;
		private readonly WebApplicationFactory<Program> _factory;

		public CatalogServiceAPIIntegrationTests(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
			_client = _factory.CreateClient();
		}

		[Fact]
		public async Task GetCategories_ReturnsSuccess()
		{
			var response = await _client.GetAsync("/api/catalog-service/categories");
			response.EnsureSuccessStatusCode();
		}
	}
}
