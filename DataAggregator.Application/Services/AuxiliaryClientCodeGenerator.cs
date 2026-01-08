using DataAggregator.Application.Services.Abstractions;

namespace DataAggregator.Application.Services
{
    public class AuxiliaryClientCodeGenerator : IAuxiliaryClientCodeGenerator
    {
        public string Generate(string firstName, string lastName, string organizationName)
        {
            var p1 = MakePart(firstName);
            var p2 = MakePart(lastName);
            var p3 = MakeOrgPart(organizationName);
            return $"{p1}-{p2}-{p3}";
        }

        private static string MakePart(string s)
        {
            s = (s ?? string.Empty).Trim();
            if (s.Length < 4) return new string(s.Reverse().ToArray()).ToUpperInvariant().PadRight(3, 'X')[..3];

            var mid = s.Substring(1, 3);
            return new string(mid.Reverse().ToArray()).ToUpperInvariant();
        }
        private static string MakeOrgPart(string org)
        {
            org = (org ?? string.Empty).Trim();
            var letters = org
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => char.ToUpperInvariant(w[0]));

            var result = new string(letters.ToArray());
            return string.IsNullOrWhiteSpace(result) ? "X" : result;
        }
    }
}
