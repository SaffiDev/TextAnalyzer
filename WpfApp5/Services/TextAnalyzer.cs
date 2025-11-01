using System;
using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WpfApp5.Models;
namespace WpfApp5.Services;

public static class TextAnalyzer
{
    // Только строчные + русские — ToLowerInvariant сделает остальное
    private static readonly FrozenSet<char> Vowels = FrozenSet.ToFrozenSet(
    [
        'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я',
        'a', 'e', 'i', 'o', 'u', 'y'
    ]);

    private static readonly FrozenSet<char> Consonants = FrozenSet.ToFrozenSet(
    [
        'б','в','г','д','ж','з','й','к','л','м','н','п','р','с','т','ф','х','ц','ч','ш','щ',
        'b','c','d','f','g','h','j','k','l','m','n','p','q','r','s','t','v','w','x','z'
    ]);

    public static ValueTask<TextAnalysisResult> AnalyzeAsync(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return ValueTask.FromResult(TextAnalysisResult.Empty);

        return ValueTask.FromResult(Analyze(text));
    }

    private static TextAnalysisResult Analyze(ReadOnlySpan<char> span)
    {
        int vowels = 0, consonants = 0, digits = 0, spaces = 0, special = 0;

        foreach (var c in span)
        {
            if (char.IsWhiteSpace(c))
                spaces++;
            else if (char.IsDigit(c))
                digits++;
            else if (char.IsLetter(c))
            {
                char lower = char.ToLowerInvariant(c);
                if (Vowels.Contains(lower))
                    vowels++;
                else if (Consonants.Contains(lower))
                    consonants++;
                else
                    special++;
            }
            else
                special++;
        }

        return new(vowels, consonants, span.Length, digits, spaces, special);
    }
    // Перегрузка для string
    private static TextAnalysisResult Analyze(string text) => Analyze(text.AsSpan());
}