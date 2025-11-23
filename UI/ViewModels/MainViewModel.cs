using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CheckFlow.Reports.Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CheckFlow.Reports.UI.ViewModels;

public partial class MainViewModel(
    IZipService zipService,
    IChecklistService checklistService,
    IPhotoService photoService,
    IPdfService pdfService,
    Window window) : ViewModelBase
{
    [ObservableProperty] private string? selectedZipPath;

    [ObservableProperty] private string statusMessage = string.Empty;

    public bool CanGenerate => !string.IsNullOrEmpty(SelectedZipPath);

    partial void OnSelectedZipPathChanged(string? value)
    {
        OnPropertyChanged(nameof(CanGenerate));
    }

    [RelayCommand]
    private async Task SelectZipAsync()
    {
        try
        {
            var files = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Selecione o arquivo ZIP exportado",
                AllowMultiple = false,
                FileTypeFilter =
                [
                    new FilePickerFileType("ZIP") { Patterns = new[] { "*.zip" } }
                ]
            });

            if (files.Count > 0)
            {
                SelectedZipPath = files[0].Path.LocalPath;
                StatusMessage = "Arquivo selecionado.";
            }
            else
            {
                StatusMessage = "Seleção cancelada.";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = "Erro ao abrir seletor: " + ex.Message;
        }
    }

    [RelayCommand]
    private async Task GenerateReportAsync()
    {
        if (string.IsNullOrEmpty(SelectedZipPath))
        {
            StatusMessage = "Selecione um ZIP antes de gerar.";
            return;
        }

        string? tempDir = null;
        try
        {
            StatusMessage = "Extraindo ZIP...";
            tempDir = await zipService.ExtractZipAsync(SelectedZipPath);

            StatusMessage = "Carregando checklist...";
            var checklist = await checklistService.LoadChecklistAsync(tempDir);

            StatusMessage = "Validando checklist...";
            checklistService.ValidateChecklist(checklist);

            StatusMessage = "Validando fotos (checando existência)...";
            await photoService.ValidatePhotosAsync(checklist, tempDir);

            StatusMessage = "Gerando PDF...";
            var outputFolder = Path.GetDirectoryName(SelectedZipPath) ??
                               Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var safeName = string.Concat(checklist.Name.Split(Path.GetInvalidFileNameChars()));
            var outputFile = Path.Combine(outputFolder, $"{safeName}_report.pdf");

            await pdfService.GeneratePdfAsync(checklist, tempDir, outputFolder, outputFile);

            StatusMessage = $"Concluído: {outputFile}";
        }
        catch (Exception ex)
        {
            StatusMessage = "Erro: " + ex.Message;
        }
        finally
        {
            if (!string.IsNullOrEmpty(tempDir) && Directory.Exists(tempDir))
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // Ignore cleanup errors, temp folder removal is best-effort only.
                }
        }
    }
}