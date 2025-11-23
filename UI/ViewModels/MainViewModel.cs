using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CheckFlow.Reports.UI.ViewModels;

public partial class MainViewModel(Window window) : ViewModelBase
{
    // private readonly IReportService _reportService;

    [ObservableProperty] private string? selectedZipPath;

    [ObservableProperty] private string statusMessage = string.Empty;

    public bool CanGenerate => !string.IsNullOrEmpty(SelectedZipPath);

    [RelayCommand]
    private async Task SelectZipAsync()
    {
        var file = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Selecione o arquivo ZIP exportado",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("ZIP") { Patterns = new[] { "*.zip" } }
            ]
        });

        if (file.Count > 0)
        {
            SelectedZipPath = file[0].Path.LocalPath;
            StatusMessage = "Arquivo selecionado.";
            OnPropertyChanged(nameof(CanGenerate));
        }
    }

    [RelayCommand]
    private async Task GenerateReportAsync()
    {
        if (SelectedZipPath is null) return;
        StatusMessage = "Processando...";

        try
        {
            // var outPath = await _reportService.ProcessZipAndGeneratePdfAsync(SelectedZipPath);
            StatusMessage = "Concluído: Path";
        }
        catch (Exception ex)
        {
            StatusMessage = "Erro: " + ex.Message;
        }
    }
}