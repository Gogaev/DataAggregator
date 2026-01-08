using DataAggregator.Application.Models;

namespace DataAggregator.Application.Services.Abstractions
{
    public interface ITenantDataSource
    {
        int TenantId { get; }
        Task<IReadOnlyList<QuietCustomerCandidate>> GetQuietCustomersAsync(
            string tenantName,
            DateTime fromUtc,
            DateTime toUtcExclusive,
            CancellationToken ct);
    }
}
