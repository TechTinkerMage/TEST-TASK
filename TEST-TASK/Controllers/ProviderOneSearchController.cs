using Microsoft.AspNetCore.Mvc;
using TEST_TASK.ProviderOne;

namespace TEST_TASK.Controllers
{
    [ApiController]
    [Route("provider-one/api/v1")]
    public class ProviderOneSearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public ProviderOneSearchController(IServiceProvider provider)
        {
            _searchService = provider.GetServices<ISearchServiceProvider>().First(x => x.GetType() == typeof(ProviderOneSearchService));
        }

        [HttpPost("search")]
        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            return await _searchService.SearchAsync(request, cancellationToken);
        }

        [HttpGet("ping")]
        public async Task<IActionResult> PingAsync(CancellationToken cancellationToken)
        {
            var isAvailable = await _searchService.IsAvailableAsync(cancellationToken);

            return isAvailable ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
