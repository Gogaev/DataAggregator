using System;
using System.Collections.Generic;

namespace DataAggregator.Infrastructure.Entities;

public partial class Events_145
{
    public decimal Id { get; set; }

    public string CustomerId { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public string EventType { get; set; } = null!;
}
