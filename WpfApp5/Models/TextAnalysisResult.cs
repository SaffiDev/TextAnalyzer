namespace WpfApp5.Models;

public record TextAnalysisResult(
    int VowelsCount,
    int ConsonantsCount,
    int TotalCharacters,
    int DigitsCount,
    int SpacesCount,
    int SpecialCharactersCount)
{
    public static readonly TextAnalysisResult Empty = new(0, 0, 0, 0, 0, 0);
}