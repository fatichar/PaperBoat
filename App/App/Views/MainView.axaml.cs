using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using App.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
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

    private void LoadDocumentButton_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void BrowseButton_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select a document",
            Filters = new List<FileDialogFilter>
            {
                new () { Name = "PDF Files", Extensions = { "pdf" } },
                new () { Name = "Image Files", Extensions = { "png" } }
            }
        };
        Task.Run(async () =>
        {
            var result = await dialog.ShowAsync(GetMainWindow());
            if (result != null && result.Any())
            {
                LoadDocument(result[0]);
            }
        });
    }

    private void LoadDocument(string path)
    {
        var doc = new PdfDocument();
        doc.LoadFromFile(path);

        var imageStream = doc.SaveAsImage(0, PdfImageType.Bitmap);
        var bmp = new Bitmap(imageStream);

        Dispatcher.UIThread.InvokeAsync(() => DocImage.Source = bmp);
    }

    private Window? GetMainWindow()
    {
        return this.GetVisualRoot() as Window;
    }
}