using System.Configuration;
using System.Diagnostics;

namespace AlphaPersonel.Services.Api;

internal static class ReportService
{
    private static readonly string? ApiUrl = ConfigurationManager.AppSettings["api"];

    private static readonly string? DocxUrl = ConfigurationManager.AppSettings["docx"];

    private static void SaveReport(Stream inputStream, string reportName)
    {
        // Получить путь рабочего стола
        var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


        /*SaveFileDialog sf = new()
        {
            FileName = $"{reportName}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}",
            Filter = "DocX|*.docx",
            DefaultExt = ".docx",
        };
        */
        //if (sf.ShowDialog() != true) return;
        var file = path + $"\\Документы-Программы\\{reportName.Replace(" ", "_")}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.docx";
        if (Directory.Exists(path + $"\\Документы-Программы"))
        {
            using FileStream outputFileStream = new(file, FileMode.Create);
            inputStream.CopyTo(outputFileStream);
        }
        else
        {
            
            Directory.CreateDirectory(path + $"\\Документы-Программы");
            using FileStream outputFileStream = new(file, FileMode.Create);
            inputStream.CopyTo(outputFileStream);

        }
        inputStream.Dispose();
        //OLD "C:\Program Files\Microsoft Office\Office14\WINWORD.EXE" 
        //NEW "C:\Program Files\Microsoft Office\root\Office16\WINWORD.EXE"
        if(DocxUrl != null)
            _ = Process.Start(DocxUrl, file);
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

