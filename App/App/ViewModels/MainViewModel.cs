using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using PaperBoat.Model;
using Serilog;
using PdfLibCore;
using PdfLibCore.Enums;

namespace App.ViewModels;

public partial class MainViewModel(AppConfig appConfig) : ViewModelBase
{
    #region Constants
    private const string EXTRACT_API = "extract";
    #endregion

    #region Properties
    [ObservableProperty]
    private bool _isDocumentLoaded;

    [ObservableProperty]
    private int _pageCount = 0;

    [ObservableProperty]
    private int _currentPageIndex = 0;

    [ObservableProperty] private bool _canPrevious;
    [ObservableProperty] private bool _canNext;
    #endregion Properties

    public MainViewModel() : this(new AppConfig())
    {
    }

    private PdfDocument? CurrentDoc { get; set; }

    private HttpClient Client { get; } = new()
    {
        BaseAddress = new Uri(appConfig.BaseUrl)
    };

    public void LoadDocument(string path)
    {
        CurrentDoc = new PdfDocument(path);
        PageCount = CurrentDoc?.Pages.Count?? 0;
        IsDocumentLoaded = true;
    }

    public Bitmap GetDocImage(int pageIndex)
    {
        if (CurrentDoc == null)
        {
            throw new Exception("Document not loaded");
        }

        var pdfPage = CurrentDoc.Pages[pageIndex];
        double dpiX = 300D, dpiY = 300D;
        var pageWidth = (int) (dpiX * pdfPage.Size.Width / 72);
        var pageHeight = (int) (dpiY * pdfPage.Size.Height / 72);

        using var bitmap = new PdfiumBitmap(pageWidth, pageHeight, true);
        pdfPage.Render(bitmap, PageOrientations.Normal, RenderingFlags.LcdText);

        return new Bitmap(bitmap.AsBmpStream());
    }

    public async Task<ExtractionResponse> ExtractAsync()
    {
        if (!IsDocumentLoaded)
        {
            throw new InvalidOperationException("Document not loaded");
        }
        var request = new ExtractionRequest
        {
            FileType = "pdf",
            DocType = "Loan Application",
            Content = ToBase64String(CurrentDoc!)
        };

        var requestJson = JsonContent.Create(request);
        var response = await Client.PostAsync(EXTRACT_API, requestJson);
        var content = await response.Content.ReadAsStringAsync();
        return ExtractionResponse.Parser.ParseJson(content);
    }

    private static string ToBase64String(PdfDocument doc)
    {
        using var ms = new MemoryStream();
        doc.Save(ms);
        return Convert.ToBase64String(ms.ToArray());
    }
}