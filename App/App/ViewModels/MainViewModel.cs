using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using PaperBoat.Model;
using Spire.Pdf;
using Spire.Pdf.Graphics;

namespace App.ViewModels;

public partial class MainViewModel(AppConfig appConfig) : ViewModelBase
{
    private const string EXTRACT_API = "extract";

    private readonly PdfDocument _currentDoc = new();
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri(appConfig.BaseUrl)
    };

    public void LoadDocument(string path)
    {
        _currentDoc.LoadFromFile(path);
        IsDocumentLoaded = true;
    }

    [ObservableProperty]
    private bool _isDocumentLoaded;

    public Bitmap GetDocImage(int pageIndex)
    {
        var imageStream = _currentDoc.SaveAsImage(pageIndex, PdfImageType.Bitmap);
        var bmp = new Bitmap(imageStream);
        return bmp;
    }

    public async Task<ExtractionResponse> ExtractAsync()
    {
        var request = new ExtractionRequest
        {
            FileType = "pdf",
            DocType = "Loan Application",
            Content = ToBase64String(_currentDoc)
        };

        var requestJson = JsonContent.Create(request);
        var response = await _httpClient.PostAsync(EXTRACT_API, requestJson);
        var content = await response.Content.ReadAsStringAsync();
        return ExtractionResponse.Parser.ParseJson(content);
    }

    private static string ToBase64String(PdfDocument doc)
    {
        using var ms = new MemoryStream();
        doc.SaveToStream(ms);
        return Convert.ToBase64String(ms.ToArray());
    }
}