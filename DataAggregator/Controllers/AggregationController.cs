using DataAggregator.Application.Models;
using DataAggregator.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DataAggregator.Controllers
{
    [ApiController]
    [Route("api/aggregation")]
    public class AggregationController : ControllerBase
    {

        private const int DEFAULT_YEAR = 2024;
        private const int DEFAULT_MONTH = 4;
        private readonly IRunAggregationUseCase _useCase;

        public AggregationController(IRunAggregationUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("run")]
        public async Task<ActionResult<RunAggregationResult>> Run(CancellationToken ct)
        {
            var from = new DateTime(DEFAULT_YEAR, DEFAULT_MONTH, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = from.AddMonths(1);

            var result = await _useCase.RunAsync(from, to, ct);

            return Ok(result);
        }
    }
}
