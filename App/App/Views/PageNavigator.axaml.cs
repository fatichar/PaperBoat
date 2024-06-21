using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App.Views;

public partial class PageNavigator : UserControl
{
    #region Properties
    public event EventHandler? OnNext;
    public event EventHandler? OnPrevious;
    public new event PropertyChangedEventHandler? PropertyChanged;
    public static readonly DirectProperty<PageNavigator, int> CurrentPageProperty =
        AvaloniaProperty.RegisterDirect<PageNavigator, int>(
            nameof(CurrentPage),
            o => o.CurrentPage,
            (o, v) => o.CurrentPage = v);

    public static readonly DirectProperty<PageNavigator, int> PageCountProperty =
        AvaloniaProperty.RegisterDirect<PageNavigator, int>(
            nameof(PageCount),
            o => o.PageCount,
            (o, v) => o.PageCount = v);

    private int _currentPage;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage == value) return;

            _currentPage = value;
            OnPropertyChanged();
            UpdateDisplayText();
            UpdateButtonStates();
        }
    }

    private int _pageCount;
    public int PageCount
    {
        get => _pageCount;
        set
        {
            if (_pageCount == value) return;

            _pageCount = value;
            OnPropertyChanged();
            UpdateDisplayText();
            UpdateButtonStates();
        }
    }

    private void UpdateDisplayText()
    {
        _displayText = $"Page {CurrentPage + 1} of {PageCount}";
        OnPropertyChanged(nameof(DisplayText));
        PageStateBlock.Text = _displayText;
    }

    private string _displayText = "Page 0 of 0";
    public string DisplayText
    {
        get => _displayText;
        set => _displayText = value;
    }

    #endregion Properties
    public PageNavigator()
    {
        InitializeComponent();
        // DataContext = this;
    }

    private void PreviousButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OnPrevious?.Invoke(this, e);
        if (CurrentPage > 0)
        {
            _currentPage--;
        }
    }

    private void NextButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OnNext?.Invoke(this, e);
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void UpdateButtonStates()
    {
        PreviousButton.IsEnabled = CurrentPage > 0;
        NextButton.IsEnabled = CurrentPage < PageCount - 1;
    }
}