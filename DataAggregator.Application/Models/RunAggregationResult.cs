using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAggregator.Application.Models
{
    public class RunAggregationResult
    {
        public DateTime FromUtc { get; set; }
        public DateTime ToUtcExclusive { get; set; }
        public int TenantsProcessed { get; set; }
        public int QuietCustomersFound { get; set; }
        public int NotificationsEnqueued { get; set; }
        public IReadOnlyDictionary<int, int> QuietCustomersByTenant { get; set; } = new Dictionary<int, int>();
    }
}
