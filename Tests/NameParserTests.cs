using DataAggregator.Application.Helpers;
using FluentAssertions;

namespace Tests
{
    public class NameParserTests
    {
        [Theory]
        [InlineData("John Smith", "John", "Smith")]
        [InlineData("John", "John", "")]
        [InlineData("  John   Ronald   Reuel Tolkien  ", "John", "Ronald Reuel Tolkien")]
        [InlineData("", "", "")]
        public void ParseFullName_ReturnsExpected(string input, string expectedFirst, string expectedLast)
        {
            var (first, last) = NameParserHelper.ParseFullName(input);

            first.Should().Be(expectedFirst);
            last.Should().Be(expectedLast);
        }

        [Fact]
        public void ParseFullName_Null_ReturnsEmpty()
        {
            var (first, last) = NameParserHelper.ParseFullName(null);

            first.Should().Be("");
            last.Should().Be("");
        }
    }
}
