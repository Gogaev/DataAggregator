using System;
using System.Collections.Generic;

namespace DataAggregator.Infrastructure.Entities;

public partial class Tenant
{
    public int Id { get; set; }

    public string OrganisationName { get; set; } = null!;
}
