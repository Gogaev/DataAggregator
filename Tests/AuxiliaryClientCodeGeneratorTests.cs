using DataAggregator.Application.Services;
using FluentAssertions;

namespace Tests
{
    public class AuxiliaryClientCodeGeneratorTests
    {
        private readonly AuxiliaryClientCodeGenerator _sut = new();

        [Theory]
        [InlineData("Jack", "Sparrow", "Jack Sparrow Warship", "KCA-RAP-JSW")]
        [InlineData("Robert", "Martin", "Clean Code", "EBO-TRA-CC")]
        public void Generate_ReturnsExpectedFinHash(string firstName, string lastName, string org, string expected)
        {
            var result = _sut.Generate(firstName, lastName, org);
            result.Should().Be(expected);
        }

        [Fact]
        public void Generate_TrimsAndUppercases()
        {
            var result = _sut.Generate("  jack ", " sparrow  ", "  Jack Sparrow Warship  ");
            result.Should().Be("KCA-RAP-JSW");
        }

        [Fact]
        public void Generate_OrgInitials_UsesFirstLettersOfWords()
        {
            var result = _sut.Generate("Jack", "Sparrow", "Jack Sparrow Warship");
            result.Should().EndWith("-JSW");
        }
    }
}
