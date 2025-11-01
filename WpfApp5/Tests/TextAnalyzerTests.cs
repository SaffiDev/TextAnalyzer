using System.Threading.Tasks;
using FluentAssertions;
using WpfApp5.Models;
using WpfApp5.Services;
using Xunit;

namespace WpfApp5.Tests;

public class TextAnalyzerTests
{
    [Fact]
    public async Task AnalyzeAsync_Empty_ReturnsEmpty()
    {
        var result = await TextAnalyzer.AnalyzeAsync("");
        result.Should().Be(TextAnalysisResult.Empty);
    }

    [Theory]
    [InlineData("привет", 2, 4, 6, 0, 0, 0)]
    [InlineData("hello", 2, 3, 5, 0, 0, 0)]
    [InlineData("Привет, мир! 123 @#$", 3, 6, 20, 3, 3, 5)]
    public async Task AnalyzeAsync_CountsCorrectly(
        string text, int vowels, int consonants, int total,
        int digits, int spaces, int special)
    {
        var result = await TextAnalyzer.AnalyzeAsync(text);

        result.VowelsCount.Should().Be(vowels);
        result.ConsonantsCount.Should().Be(consonants);
        result.TotalCharacters.Should().Be(total);
        result.DigitsCount.Should().Be(digits);
        result.SpacesCount.Should().Be(spaces);
        result.SpecialCharactersCount.Should().Be(special);
    }
}