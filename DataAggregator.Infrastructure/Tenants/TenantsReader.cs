using DataAggregator.Application.Services.Abstractions;
using DataAggregator.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace DataAggregator.Infrastructure.Tenants
{
    public class TenantsReader : ITenantsReader
    {
        private readonly AggregatorDbContext _context;

        public TenantsReader(AggregatorDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<(int TenantId, string TenantName)>> GetTenantsAsync(CancellationToken ct)
        {
            return await _context.Tenants
                .AsNoTracking()
                .Select(t => new ValueTuple<int, string>(t.Id, t.OrganisationName))
                .ToListAsync(ct);
        }
    }
}
