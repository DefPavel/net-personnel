using System.Configuration;

namespace AlphaPersonel.Services.Api;

internal static class ReportService
{
    private static readonly string? ApiUrl = ConfigurationManager.AppSettings["api"];
    private static void SaveReport(Stream inputStream, string reportName)
    {
        // Получить путь рабочего стола
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


        /*SaveFileDialog sf = new()
        {
            FileName = $"{reportName}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}",
            Filter = "DocX|*.docx",
            DefaultExt = ".docx",
        };
        */
        //if (sf.ShowDialog() != true) return;
        if(Directory.Exists(path + $"\\Документы-Программы"))
        {
            using FileStream outputFileStream = new(path + $"\\Документы-Программы\\{reportName}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.docx", FileMode.Create);
            inputStream.CopyTo(outputFileStream);
        }
        else
        {
            Directory.CreateDirectory(path + $"\\Документы-Программы");
            using FileStream outputFileStream = new(path + $"\\Документы-Программы\\{reportName}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.docx", FileMode.Create);
            inputStream.CopyTo(outputFileStream);

        }
        MessageBox.Show("Отчет создан! Путь:" + path + $"\\Документы-Программы\\{reportName}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.docx");
       
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

