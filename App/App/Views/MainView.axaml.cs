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
    private MainViewModel? Context => (MainViewModel?)DataContext;

    public MainView()
    {
        InitializeComponent();
    }

    private async void LoadDocumentButton_OnClick(object? sender, RoutedEventArgs e)
    {
        LoadDocument(DocumentPathBox.Text);
    }

    private async void BrowseButton_Click(object? sender, RoutedEventArgs e)
    {
        var pdfFilePath = await OpenPdfFileDialog(GetMainWindow());
        if (!string.IsNullOrEmpty(pdfFilePath))
        {
            DocumentPathBox.Text = pdfFilePath;
        }
    }

    private async Task<string?> OpenPdfFileDialog(TopLevel parent)
    {

        var filePickerOpenOptions = new FilePickerOpenOptions
        {
            Title = "Open PDF File",
            FileTypeFilter = new List<FilePickerFileType>
            {
                new FilePickerFileType("PDF Files")
                {
                    Patterns = new[] { "*.pdf" }
                }
            },
            AllowMultiple = false
        };

        var files = await parent.StorageProvider.OpenFilePickerAsync(filePickerOpenOptions);

        if (files != null && files.Count > 0)
        {
            return files[0].Path.LocalPath;
        }

        return null;
    }

    private void LoadDocument(string path)
    {
        var doc = new PdfDocument();
        doc.LoadFromFile(path);

        var imageStream = doc.SaveAsImage(0, PdfImageType.Bitmap);
        var bmp = new Bitmap(imageStream);

        Dispatcher.UIThread.InvokeAsync(() => DocImage.Source = bmp);
    }

    private Window GetMainWindow()
    {
        return this.GetVisualRoot() as Window ?? throw new System.Exception("MainWindow not found");
    }
}