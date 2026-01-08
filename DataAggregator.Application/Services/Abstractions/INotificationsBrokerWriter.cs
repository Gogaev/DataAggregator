using DataAggregator.Application.Models;

namespace DataAggregator.Application.Services.Abstractions
{
    public interface INotificationsBrokerWriter
    {
        Task<int> EnqueueAsync(IEnumerable<Notification> items, CancellationToken ct);
    }
}
