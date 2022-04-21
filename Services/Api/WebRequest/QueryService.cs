using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net;

namespace AlphaPersonel.Services.Api;
/**
 * Устаревший метод замени потом на WebClient в ServiceClient
 */
internal static class QueryService
{
    private static readonly string? ApiUrl = ConfigurationManager.AppSettings["api"];

    #region Обобщение Десериализации С Токеном
    public static async ValueTask<ObservableCollection<T>> JsonDeserializeWithToken<T>(string token, string queryUrl, string httpMethod) where T : new()
    {
#pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = httpMethod;                                                        // Выбираем метод запроса
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        using var response = await req.GetResponseAsync();

        await using var responseStream = response.GetResponseStream();
        using StreamReader reader = new(responseStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<ObservableCollection<T>>(await reader.ReadToEndAsync())
             ?? throw new NullReferenceException();// Возвращаем json информацию которая пришла 
    }

    public static async ValueTask<T> JsonDeserializeWithObject<T>(string token, string queryUrl, string httpMethod)
    {
#pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = httpMethod;                                                        // Выбираем метод запроса
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        using var response = await req.GetResponseAsync();

        await using var responseStream = response.GetResponseStream();
        using StreamReader reader = new(responseStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<T>(await reader.ReadToEndAsync())
             ?? throw new NullReferenceException(); ;    // Возвращаем json информацию которая пришла 
    }

    public static async ValueTask<T> JsonDeserializeWithObjectAndParam<T>(string token, string queryUrl, string httpMethod, T obj)
    {
#pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014 // Тип или член устарел
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
        using StreamReader reader = new(responseStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<T>(await reader.ReadToEndAsync())
             ?? throw new NullReferenceException(); ;    // Возвращаем json информацию которая пришла 
    }

    public static async ValueTask<T> JsonObjectWithToken<T>(string token, string queryUrl, string httpMethod) where T : new()
    {
#pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = httpMethod;                                                        // Выбираем метод запроса
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        using var response = await req.GetResponseAsync();

        await using var responseStream = response.GetResponseStream();
        using StreamReader reader = new(responseStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<T>(await reader.ReadToEndAsync())
             ?? throw new NullReferenceException(); ;    // Возвращаем json информацию которая пришла 
    }
    #endregion

    #region Обобщение Десериализации С Токеном и параметром
    public static async ValueTask<ObservableCollection<T>> JsonDeserializeWithTokenAndParam<T>(string token, string queryUrl, string httpMethod, T obj) where T : new()
    {
#pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014 // Тип или член устарел
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
        using StreamReader reader = new(responseStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<ObservableCollection<T>>(await reader.ReadToEndAsync())
             ?? throw new NullReferenceException();     // Возвращаем json информацию которая пришла 
    }
    #endregion



    #region Обобщение Сериализации С Токеном
    public static async ValueTask JsonSerializeWithToken<T>(string token, string queryUrl, string httpMethod, T obj)
    {
#pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);// Создаём запрос
#pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = httpMethod;
        req.Headers.Add("auth-token", token);
        // Выбираем метод запроса
        req.Accept = "application/json";
        var json = JsonSerializer.Serialize(obj);
        await using (StreamWriter streamWriter = new(req.GetRequestStream()))
        {
            req.ContentType = "application/json";
            await streamWriter.WriteAsync(json);
            // Записывает тело
            streamWriter.Close();
        }
        using var response = await req.GetResponseAsync();
        await using var responseStream = response.GetResponseStream();
        {
            using StreamReader reader = new(responseStream, Encoding.UTF8);
        }
    }
    #endregion
}

