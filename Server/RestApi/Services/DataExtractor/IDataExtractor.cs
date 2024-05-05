﻿using RestApi.Controllers;
using RestApi.Models;
using System.Reflection.Metadata;

namespace RestApi.Services.DataExtractor
{
    public interface IDataExtractor
    {
        Models.Document Extract(string data, string docType, ExtractionRequest extractionRequest);
    }
}
