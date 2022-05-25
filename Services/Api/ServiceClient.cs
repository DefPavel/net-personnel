using System.Net.Http;
namespace AlphaPersonel.Services;


/*
    Вообще лучше конечно работать через httpClient
    Нужно потом переделать с WebRequest на httpClient

// Пример использования POST
using ServiceClient? client = new("http://localhost:8080");
string encryptedPass = CustomAes256.Encrypt("root", "8UHjPgXZzXDgkhqV2QCnooyJyxUzfJrO");
Users user = new()
{
    UserName = "1497",
    Password = encryptedPass,
    IdModules = ModulesProject.Personel
};
try
{
    var userResponse = await client.PostAsync<Users>("api/auth", user);
    MessageBox.Show(userResponse.Token);
}
catch (HttpRequestException ex)
{
    MessageBox.Show(ex.Message);
            
}

 
 */
internal class ServiceClient : IDisposable
{
    private readonly TimeSpan _timeout;
    private HttpClient _httpClient;
    private HttpClientHandler _httpClientHandler;
    private readonly string _baseUrl;
    private const string ClientUserAgent = "my-api-client-v1";
    private const string MediaTypeJson = "application/json";

#pragma warning disable CS8618
    public ServiceClient(string baseUrl, TimeSpan? timeout = null)
#pragma warning restore CS8618
    {
        _baseUrl = NormalizeBaseUrl(baseUrl);
        _timeout = timeout ?? TimeSpan.FromSeconds(90);
    }

    private async Task<string> PostAsync(string url, object input)
    {
        EnsureHttpClientCreated();

        using var requestContent = new StringContent(ConvertToJsonString(input), Encoding.UTF8, MediaTypeJson);
        using var response = await _httpClient.PostAsync(url, requestContent);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<TResult> PostAsync<TResult>(string url, object input) where TResult : class, new()
    {
        var strResponse = await PostAsync(url, input);

        return JsonSerializer.Deserialize<TResult>(strResponse) 
            ?? throw new NullReferenceException();
    }


    public async Task<TResult> GetAsync<TResult>(string url) where TResult : class, new()
    {
        var strResponse = await GetAsync(url);

        return JsonSerializer.Deserialize<TResult>(strResponse) 
            ?? throw new NullReferenceException();
    }

    private async Task<string> GetAsync(string url)
    {
        EnsureHttpClientCreated();

        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PutAsync(string url, object input)
    {
        return await PutAsync(url, new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, MediaTypeJson));
    }

    private async Task<string> PutAsync(string url, HttpContent content)
    {
        EnsureHttpClientCreated();

        using var response = await _httpClient.PutAsync(url, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> DeleteAsync(string url)
    {
        EnsureHttpClientCreated();

        using var response = await _httpClient.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public void Dispose()
    {
        _httpClientHandler.Dispose();
        _httpClient.Dispose();
    }

    private void CreateHttpClient()
    {
        _httpClientHandler = new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip
        };

        _httpClient = new HttpClient(_httpClientHandler, false)
        {
            Timeout = _timeout
        };

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(ClientUserAgent);

        if (!string.IsNullOrWhiteSpace(_baseUrl))
        {
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeJson));
    }

    private void EnsureHttpClientCreated()
    {
        if (_httpClient == null)
        {
            CreateHttpClient();
        }
    }

    private static string ConvertToJsonString(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    private static string NormalizeBaseUrl(string url)
    {
        return url.EndsWith("/") ? url : url + "/";
    }
}

