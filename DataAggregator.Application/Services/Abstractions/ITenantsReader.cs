namespace DataAggregator.Application.Services.Abstractions
{
    public interface ITenantsReader
    {
        Task<IReadOnlyList<(int TenantId, string TenantName)>> GetTenantsAsync(CancellationToken ct);
    }
}
