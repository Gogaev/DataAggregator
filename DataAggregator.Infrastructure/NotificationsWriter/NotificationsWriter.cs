using DataAggregator.Application.Models;
using DataAggregator.Application.Services.Abstractions;
using DataAggregator.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace DataAggregator.Infrastructure.NotificationsWriter
{
    public class NotificationsWriter : INotificationsBrokerWriter
    {
        private readonly AggregatorDbContext _context;

        public NotificationsWriter(AggregatorDbContext context)
        {
            _context = context;
        }

        public async Task<int> EnqueueAsync(IEnumerable<Notification> items, CancellationToken ct)
        {
            await using var tx = await _context.Database.BeginTransactionAsync(ct);

            var affected = 0;
            foreach (var i in items)
            {
                affected += await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO dbo.NotificationsBroker (Email, FirstName, LastName, FinHash)
                VALUES ({i.Email}, {i.FirstName}, {i.LastName}, {i.FinHash});
                 ", ct);
            }

            await tx.CommitAsync(ct);
            return affected;
        }
    }
}
