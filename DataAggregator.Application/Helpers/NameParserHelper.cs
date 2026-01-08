namespace DataAggregator.Application.Helpers
{
    public static class NameParserHelper
    {
        public static (string First, string Last) ParseFullName(string? fullName)
        {
            var s = (fullName ?? string.Empty).Trim();
            if (s.Length == 0) return ("", "");

            var parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return (parts[0], "");
            return (parts[0], string.Join(' ', parts.Skip(1)));
        }
    }
}
