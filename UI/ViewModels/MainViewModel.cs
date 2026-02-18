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
	[ObservableProperty] private bool isBusy;

	[ObservableProperty] private string? selectedZipPath;

	[ObservableProperty] private string statusMessage = string.Empty;

	public bool CanGenerate => !string.IsNullOrEmpty(SelectedZipPath) && !IsBusy;

	partial void OnIsBusyChanged(bool value)
	{
		OnPropertyChanged(nameof(CanGenerate));
	}

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
				Title = "Selecione o arquivo ZIP",
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

	private async Task RunStepAsync(string message, Action action)
	{
		StatusMessage = message;
		await Task.Run(action);
	}

	[RelayCommand]
	private async Task GenerateReportAsync()
	{
		if (string.IsNullOrEmpty(SelectedZipPath))
		{
			StatusMessage = "É necessário selecionar o ZIP antes de gerar o PDF.";
			return;
		}

		IsBusy = true;
		string? tempDir = null;

		try
		{
			await RunStepAsync("Extraindo ZIP...", () => { tempDir = zipService.ExtractZip(SelectedZipPath); });

			StatusMessage = "Carregando checklist...";
			var checklist = await checklistService.LoadChecklistAsync(tempDir!);

			StatusMessage = "Validando checklist...";
			checklistService.ValidateChecklist(checklist);

			await RunStepAsync("Validando fotos...", () =>
				photoService.UpdatePhotoStatuses(checklist, tempDir!));

			var outputFolder = Path.GetDirectoryName(SelectedZipPath)
			                   ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			var safeName = string.Concat(checklist.Title.Split(Path.GetInvalidFileNameChars()));
			if (string.IsNullOrWhiteSpace(safeName))
			{
				safeName = "checklist";
			}

			var outputFile = Path.Combine(outputFolder, $"{safeName}_report.pdf");

			await RunStepAsync("Gerando PDF...", () =>
				pdfService.GeneratePdf(checklist, tempDir!, outputFolder, outputFile));

			StatusMessage = $"Concluído: {outputFile}";
		}
		catch (Exception ex)
		{
			StatusMessage = "Erro ao gerar o relatório: " + ex.Message;
		}
		finally
		{
			IsBusy = false;

			if (!string.IsNullOrEmpty(tempDir) && Directory.Exists(tempDir))
			{
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
}
