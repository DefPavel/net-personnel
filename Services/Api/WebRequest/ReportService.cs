using Microsoft.Win32;
using System.Configuration;

namespace AlphaPersonel.Services.Api;

internal static class ReportService
{
    private static readonly string? ApiUrl = ConfigurationManager.AppSettings["api"];

    /*public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
    {
        DirectoryInfo info = new(filePath);
        if (!info.Exists)
        {
            info.Create();
        }
        string path = Path.Combine(filePath, fileName);
        using FileStream outputFileStream = new(path, FileMode.Create);
        inputStream.CopyTo(outputFileStream);
    }
    */

    private static void SaveReport(Stream inputStream, string reportName)
    {
        SaveFileDialog sf = new()
        {
            FileName = reportName,
            Filter = "DocX|*.docx",
            DefaultExt = ".docx",
        };
        if (sf.ShowDialog() != true) return;
        using FileStream outputFileStream = new(sf.FileName, FileMode.Create);
        inputStream.CopyTo(outputFileStream);
    }
    public static async Task JsonPostWithToken(object obj, string token, string queryUrl, string httpMethod, string reportName)
    {
#pragma warning disable SYSLIB0014
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014
        req.Method = httpMethod;                                                        // Выбираем метод запроса
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        await using (StreamWriter streamWriter = new(req.GetRequestStream()))
        {
            req.ContentType = "application/json";
            var json = JsonSerializer.Serialize(obj);
            await streamWriter.WriteAsync(json);
            // Записывает тело
            streamWriter.Close();
        }

        using var response = await req.GetResponseAsync();

        await using var responseStream = response.GetResponseStream();

        // Записываем файл в выбранный путь пользователем
        SaveReport(responseStream, reportName);
        // Записываем файл
        //SaveStreamAsFile("reports", responseStream, "228.docx");
    }
    public static async Task JsonDeserializeWithToken(string token, string queryUrl, string httpMethod, string reportName)
    {
#pragma warning disable SYSLIB0014
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014
        req.Method = httpMethod;                                                        // Выбираем метод запроса
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        using var response = await req.GetResponseAsync();

        await using var responseStream = response.GetResponseStream();

        // Записываем файл в выбранный путь пользователем
        SaveReport(responseStream, reportName);
        // Записываем файл
        //SaveStreamAsFile("reports", responseStream, "228.docx");
    }
}

