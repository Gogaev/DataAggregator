using DataAggregator.Application.Models;
using DataAggregator.Application.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace DataAggregator.Application.Services
{
    public class RunAggregationUseCase : IRunAggregationUseCase
    {
        private readonly ITenantsReader _tenants;
        private readonly IEnumerable<ITenantDataSource> _sources;
        private readonly IAuxiliaryClientCodeGenerator _codeGen;
        private readonly INotificationsBrokerWriter _broker;
        private readonly ILogger<RunAggregationUseCase> _log;

        public RunAggregationUseCase(
        ITenantsReader tenants,
        IEnumerable<ITenantDataSource> sources,
        IAuxiliaryClientCodeGenerator codeGen,
        INotificationsBrokerWriter broker,
        ILogger<RunAggregationUseCase> log)
        {
            _tenants = tenants;
            _sources = sources;
            _codeGen = codeGen;
            _broker = broker;
            _log = log;
        }

        public async Task<RunAggregationResult> RunAsync(DateTime fromUtc, DateTime toUtcExclusive, CancellationToken ct)
        {
            var tenants = await _tenants.GetTenantsAsync(ct);

            var perTenantCounts = new Dictionary<int, int>();
            var allQuiet = new List<QuietCustomerCandidate>();

            foreach (var (tenantId, tenantName) in tenants)
            {
                var src = _sources.FirstOrDefault(s => s.TenantId == tenantId);
                if (src is null)
                {
                    _log.LogWarning("No datasource registered for tenant {TenantId} ({TenantName})", tenantId, tenantName);
                    perTenantCounts[tenantId] = 0;
                    continue;
                }

                var quiet = await src.GetQuietCustomersAsync(tenantName, fromUtc, toUtcExclusive, ct);
                perTenantCounts[tenantId] = quiet.Count;
                allQuiet.AddRange(quiet);
            }

            var notifications = allQuiet
                .Where(c => !string.IsNullOrWhiteSpace(c.Email))
                .Select(c => new Notification
                {
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    FinHash = _codeGen.Generate(c.FirstName, c.LastName, c.TenantName)
                })
                .ToList();

            var enqueued = await _broker.EnqueueAsync(notifications, ct);

            return new RunAggregationResult
            {
                FromUtc = fromUtc,
                ToUtcExclusive = toUtcExclusive,
                TenantsProcessed = tenants.Count,
                QuietCustomersFound = allQuiet.Count,
                NotificationsEnqueued = enqueued,
                QuietCustomersByTenant = perTenantCounts
            };
        }
    }
}
