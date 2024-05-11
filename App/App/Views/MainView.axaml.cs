using System.Collections.Generic;
using App.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

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
                new FileDialogFilter { Name = "PDF Files", Extensions = { "pdf" } },
                new FileDialogFilter { Name = "Image Files", Extensions = { "png" } }
            }
        };
        var result = dialog.ShowAsync(GetMainWindow());
    }

    private Window? GetMainWindow()
    {
        return this.GetVisualRoot() as Window;
    }
}