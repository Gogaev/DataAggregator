using DataAggregator.Application.Models;
using DataAggregator.Application.Services.Abstractions;
using DataAggregator.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace DataAggregator.Infrastructure.Tenants
{
    public class Tenant2DataSource : ITenantDataSource
    {
        public int TenantId => 2;
        private readonly AggregatorDbContext _context;

        public Tenant2DataSource(AggregatorDbContext context) => _context = context;

        public async Task<IReadOnlyList<QuietCustomerCandidate>> GetQuietCustomersAsync(
            string tenantName,
            DateTime fromUtc,
            DateTime toUtcExclusive,
            CancellationToken ct)
        {
            var result = await _context.Customer_2s
                .AsNoTracking()
                .GroupJoin(
                    _context.Events_2s
                        .AsNoTracking()
                        .Where(e => e.EventDate >= fromUtc && e.EventDate < toUtcExclusive),
                    c => c.Id,
                    e => e.CustomerId,
                    (c, ev) => new { Customer = c, ActivityCount = ev.Count() }
                )
                .Where(x => x.ActivityCount < 3)
                .Select(x => new QuietCustomerCandidate
                {
                    TenantId = TenantId,
                    TenantName = tenantName,
                    CustomerId = x.Customer.Id.ToString(),
                    FirstName = x.Customer.GivenName ?? string.Empty,
                    LastName = x.Customer.FamilyName ?? string.Empty,
                    Email = x.Customer.Email ?? string.Empty,
                    ActivityCount = x.ActivityCount
                })
                .ToListAsync(ct);

            return result;
        }
    }
}
