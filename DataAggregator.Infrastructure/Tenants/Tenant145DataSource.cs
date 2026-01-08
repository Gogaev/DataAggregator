using DataAggregator.Application.Helpers;
using DataAggregator.Application.Models;
using DataAggregator.Application.Services.Abstractions;
using DataAggregator.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace DataAggregator.Infrastructure.Tenants
{
    public class Tenant145DataSource : ITenantDataSource
    {
        public int TenantId => 145;
        private readonly AggregatorDbContext _context;

        public Tenant145DataSource(AggregatorDbContext context) => _context = context;

        public async Task<IReadOnlyList<QuietCustomerCandidate>> GetQuietCustomersAsync(
            string tenantName,
            DateTime fromUtc,
            DateTime toUtcExclusive,
            CancellationToken ct)
        {
            var aggregated = await _context.Customer_145s
                .AsNoTracking()
                .GroupJoin(
                    _context.Events_145s
                        .AsNoTracking()
                        .Where(e => e.EventDate >= fromUtc && e.EventDate < toUtcExclusive),
                    c => c.UserId,
                    e => e.CustomerId,
                    (c, ev) => new
                    {
                        c.UserId,
                        c.Name,
                        c.Email,
                        Count = ev.Count()
                    }
                )
                .Where(x => x.Count < 3)
                .ToListAsync(ct);

            var result = aggregated
                .Select(x =>
                {
                    var (first, last) = NameParserHelper.ParseFullName(x.Name);

                    return new QuietCustomerCandidate
                    {
                        TenantId = TenantId,
                        TenantName = tenantName,
                        CustomerId = x.UserId.ToString(),
                        FirstName = first,
                        LastName = last,
                        Email = x.Email ?? string.Empty,
                        ActivityCount = x.Count
                    };
                })
                .ToList();

            return result;
        }
    }
}
