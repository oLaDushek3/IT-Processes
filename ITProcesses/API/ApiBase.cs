using System;
using System.Net.Http;

namespace ITProcesses.API;

public class ApiBase
{
    private string BaseUrl => "https://localhost:7143/";
    
    protected HttpClient HttpClient => new HttpClient(){BaseAddress = new Uri(BaseUrl)};
}