using System;
using System.Collections.Generic;

namespace DataAggregator.Infrastructure.Entities;

public partial class Customer_101
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public string? Salutation { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime? LastLoginDate { get; set; }
}
