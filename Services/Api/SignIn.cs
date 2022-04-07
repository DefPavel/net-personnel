using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AlphaPersonel.Services.Api;

internal class SignIn
{
   

    private static readonly string _ApiBaseUrl = @"http://localhost:8080/api";
    public static async Task<Users> Authentication(string username, string password)
    {
        /*Users users = new();

        using var client = new HttpClient();
        users.IdModules = ModulesProject.Personel;
        users.UserName = username;
        users.Password = CustomAes256.Encrypt(password, "8UHjPgXZzXDgkhqV2QCnooyJyxUzfJrO");

        var response = client.PostAsJsonAsync(_ApiBaseUrl+"/auth", users).Result;

        return await response.Content.ReadFromJsonAsync<Users>();
        */

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_ApiBaseUrl + "/auth");// Создаём запрос
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
#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
        return JsonSerializer.Deserialize<Users>(await reader.ReadToEndAsync());
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.

    }
}

