using System;
using System.Collections.Generic;

namespace DataAggregator.Infrastructure.Entities;

public partial class Events_101
{
    public decimal Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime EventDate { get; set; }

    public string EventType { get; set; } = null!;
}
