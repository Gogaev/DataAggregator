using DataAggregator.Application.Models;

namespace DataAggregator.Application.Services.Abstractions
{
    public interface IRunAggregationUseCase
    {
        Task<RunAggregationResult> RunAsync(DateTime fromUtc, DateTime toUtcExclusive, CancellationToken ct);
    }
}
