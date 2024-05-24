using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using App.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Spire.Pdf;
using Spire.Pdf.Graphics;

namespace App.Views;

public partial class MainView : UserControl
{
    #region Properties
    private MainViewModel Model => (MainViewModel)(DataContext ?? new MainViewModel(new AppConfig()));

    private static readonly string[] SupportedFiletypes = new[] { "*.pdf" };

    private Window MainWindow => this.GetVisualRoot() as Window ?? throw new System.Exception("MainWindow not found");
    #endregion Properties

    public MainView()
    {
        InitializeComponent();
    }

    #region Event Handlers
    private async void BrowseButton_Click(object? sender, RoutedEventArgs e)
    {
        var pdfFilePath = await OpenPdfFileDialog(MainWindow);
        if (!string.IsNullOrEmpty(pdfFilePath))
        {
            DocumentPathBox.Text = pdfFilePath;
        }
    }

    private void LoadDocumentButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var path = DocumentPathBox.Text;
        if (!string.IsNullOrEmpty(path))
        {
            Model.LoadDocument(path);
            ShowPage(0);
        }
    }

    private void ExtractButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var docPath = DocumentPathBox.Text;
        if (!string.IsNullOrEmpty(docPath))
        {
            var resultTask = Model.ExtractAsync();
        }
    }
    #endregion Event Handlers

    private void ShowPage(int pageIndex)
    {
        var bitmap = Model.GetDocImage(pageIndex);
        Dispatcher.UIThread.InvokeAsync(() => DocImage.Source = bitmap);
    }

    private static async Task<string?> OpenPdfFileDialog(TopLevel parent)
    {
        var options = new FilePickerOpenOptions
        {
            Title = "Open PDF File",
            FileTypeFilter = new List<FilePickerFileType>
            {
                new("PDF Files")
                {
                    Patterns = SupportedFiletypes
                }
            },
            AllowMultiple = false
        };

        var files = await parent.StorageProvider.OpenFilePickerAsync(options);

        if (files.Count > 0)
        {
            return files[0].Path.LocalPath;
        }

        return null;
    }

}