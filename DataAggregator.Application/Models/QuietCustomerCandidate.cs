namespace DataAggregator.Application.Models
{
    public class QuietCustomerCandidate
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int ActivityCount { get; set; }
    }
}
