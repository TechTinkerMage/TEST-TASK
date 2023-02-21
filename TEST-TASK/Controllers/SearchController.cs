using Microsoft.AspNetCore.Mvc;

namespace TEST_TASK.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost("search")]
        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            return await _searchService.SearchAsync(request, cancellationToken);
        }

        [HttpGet("routes")]
        public async Task<SearchResponse> GetRoutesAsync(CancellationToken cancellationToken)
        {
            var routes = RouteRepository.GetAll();

            return new SearchResponse
            {
                Routes = routes.ToArray(),
                MinPrice = routes.Min(r => r.Price),
                MaxPrice = routes.Max(r => r.Price),
                MinMinutesRoute = routes.Min(r => (int)r.DestinationDateTime.Subtract(r.OriginDateTime).TotalMinutes),
                MaxMinutesRoute = routes.Max(r => (int)r.DestinationDateTime.Subtract(r.OriginDateTime).TotalMinutes)
            };
        }

        [HttpGet("ping")]
        public async Task<IActionResult> PingAsync(CancellationToken cancellationToken)
        {
            var isAvailable = await _searchService.IsAvailableAsync(cancellationToken);

            return isAvailable ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
