using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using WpfApp5.Models;
using WpfApp5.Services;

namespace WpfApp5.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string inputText = string.Empty;

    [ObservableProperty]
    private TextAnalysisResult analysisResult = TextAnalysisResult.Empty;

    [ObservableProperty]
    private bool isAnalyzing;

    [ObservableProperty]
    private string statusMessage = "Введите текст и нажмите 'Анализировать'";

    // Автоматически обновляет доступность кнопок
    partial void OnInputTextChanged(string value)
    {
        AnalyzeCommand.NotifyCanExecuteChanged();
        ClearCommand.NotifyCanExecuteChanged();
    }

    //Когда меняется результат — обновляет строку "Символов: ..."
    partial void OnAnalysisResultChanged(TextAnalysisResult value)
    {
        OnPropertyChanged(nameof(ResultsSummary));
    }
    
    partial void OnIsAnalyzingChanged(bool value)
    {
        OnPropertyChanged(nameof(ResultsSummary));
    }

    public string ResultsSummary => IsAnalyzing
        ? "Анализ..."
        : $"{AnalysisResult.TotalCharacters} симв. ({AnalysisResult.TotalCharacters - AnalysisResult.SpacesCount} без пробелов) | Гласные: {AnalysisResult.VowelsCount} | Согласные: {AnalysisResult.ConsonantsCount}";

    [RelayCommand(CanExecute = nameof(CanAnalyze))]
    private async Task AnalyzeAsync()
    {
        
        IsAnalyzing = true;
        StatusMessage = "Анализируем...";

        try
        {
            await Task.Delay(300);
            var result = await TextAnalyzer.AnalyzeAsync(InputText);
            AnalysisResult = result;
            StatusMessage = "Готово!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
        finally
        {
            IsAnalyzing = false;
            AnalyzeCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand(CanExecute = nameof(CanClear))]
    private void Clear()
    {
        InputText = string.Empty;
        AnalysisResult = TextAnalysisResult.Empty;
        StatusMessage = "Очищено. Введите новый текст.";
    }

    private bool CanAnalyze => !IsAnalyzing && !string.IsNullOrWhiteSpace(InputText);
    private bool CanClear => !string.IsNullOrWhiteSpace(InputText) || AnalysisResult != TextAnalysisResult.Empty;
}
