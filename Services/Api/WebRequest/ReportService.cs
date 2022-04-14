using Microsoft.Win32;
using System.Configuration;
using System.IO;
using System.Net;

namespace AlphaPersonel.Services.Api;

internal static class ReportService
{
    private static readonly string? _ApiUrl = ConfigurationManager.AppSettings["api"];

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

    public static void SaveReport(Stream inputStream, string reportName)
    {
        SaveFileDialog sf = new()
        {
            FileName = reportName,
            Filter = "DocX|*.docx",
            DefaultExt = ".docx",
        };
        if (sf.ShowDialog() == true)
        {
            using FileStream outputFileStream = new(sf.FileName, FileMode.Create);
            inputStream.CopyTo(outputFileStream);
        }
    }
    public static async Task JsonDeserializeWithToken(string token, string queryUrl, string HttpMethod, string ReportName)
    {
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_ApiUrl + queryUrl);     // Создаём запрос
        req.Method = HttpMethod;                                                        // Выбираем метод запроса
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        using WebResponse response = await req.GetResponseAsync();

        await using Stream responseStream = response.GetResponseStream();

        // Записываем файл в выбранный путь пользователем
        SaveReport(responseStream, ReportName);
        // Записываем файл
        //SaveStreamAsFile("reports", responseStream, "228.docx");
    }
}

