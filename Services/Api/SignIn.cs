using System.Configuration;
namespace AlphaPersonel.Services;
internal static class SignIn
{
    private static readonly string ApiUrl = ConfigurationManager.AppSettings["api"]
        ?? throw new NullReferenceException("Uninitialized property: " + nameof(ApiUrl));
    public static async Task<Users> Authentication(string username, string password)
    {
        #pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + "/auth");// Создаём запрос
        #pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = "Post";
        req.Accept = "application/json";

        await using (StreamWriter streamWriter = new(req.GetRequestStream()))
        {
            req.ContentType = "application/json";
            // Второй параметр ключ , оно должно совпадать с ключом на сервере
            var encryptedPass = CustomAes256.Encrypt(password, "8UHjPgXZzXDgkhqV2QCnooyJyxUzfJrO");
            Users user = new()
            {
                UserName = username,
                Password = encryptedPass,
                IdModules = ModulesProject.Personel
            };
            var json = JsonSerializer.Serialize(user);
            await streamWriter.WriteAsync(json);
            // Записываеsт тело
            streamWriter.Close();
        }
        using var response = await req.GetResponseAsync();
        await using var responseStream = response.GetResponseStream();
        using StreamReader reader = new(responseStream, Encoding.UTF8);

        return JsonSerializer.Deserialize<Users>(await reader.ReadToEndAsync())
            ?? throw new NullReferenceException();
    }

}

