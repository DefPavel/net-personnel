using System.Configuration;
using System.IO;
using System.Net;
namespace AlphaPersonel.Services;
internal class SignIn
{
    private static readonly string _ApiUrl = ConfigurationManager.AppSettings["api"]
        ?? throw new NullReferenceException("Uninitialized property: " + nameof(_ApiUrl));
    public static async Task<Users> Authentication(string username, string password)
    {
        #pragma warning disable SYSLIB0014 // Тип или член устарел
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_ApiUrl + "/auth");// Создаём запрос
        #pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = "Post";
        req.Accept = "application/json";

        await using (StreamWriter streamWriter = new(req.GetRequestStream()))
        {
            req.ContentType = "application/json";
            // Второй параметр ключ , оно должно совпадать с ключом на сервере
            string encryptedPass = CustomAes256.Encrypt(password, "8UHjPgXZzXDgkhqV2QCnooyJyxUzfJrO");
            Users user = new()
            {
                UserName = username,
                Password = encryptedPass,
                IdModules = ModulesProject.Personel
            };
            string json = JsonSerializer.Serialize(user);
            await streamWriter.WriteAsync(json);
            // Записывает тело
            streamWriter.Close();
        }
        using WebResponse response = await req.GetResponseAsync();
        await using Stream responseStream = response.GetResponseStream();
        using StreamReader reader = new(responseStream, Encoding.UTF8);

        return JsonSerializer.Deserialize<Users>(await reader.ReadToEndAsync())
            ?? throw new NullReferenceException();
    }

}

