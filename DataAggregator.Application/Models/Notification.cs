namespace DataAggregator.Application.Models
{
    public class Notification
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? FinHash { get; set; }
    }
}
