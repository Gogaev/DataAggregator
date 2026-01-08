using System;
using System.Collections.Generic;

namespace DataAggregator.Infrastructure.Entities;

public partial class Customer_2
{
    public int Id { get; set; }

    public string? GivenName { get; set; }

    public string? FamilyName { get; set; }

    public string? JobPosition { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }
}
