using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AlphaPersonel.Services;
internal static class CustomAes256
{
    #region Конвертация из Byte в Hex String

    private static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new(ba.Length * 2);
        for (int i = 0; i < ba.Length; i++)
        {
            byte b = ba[i];
            _ = hex.AppendFormat("{0:x2}", b);
        }

        return hex.ToString();
    }

    #endregion

    #region Кодирование из String в Hex

    private static byte[] StringToByteArray(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }

        return bytes;
    }
    #endregion

    #region  Кодирование данных Формата IV : Данные
    public static string Encrypt(string plainText, string key)
    {
        // Проверяем аргуметы 
        if (plainText is not { Length: > 0 })
        {
            throw new ArgumentNullException("Текст кодировки является пустым");
        }

        if (key is not { Length: > 0 })
        {
            throw new ArgumentNullException("Ключ кодировки является пустым");
        }

        byte[] encrypted;
        byte[] iv;
        // Создайте объект RijndaelManaged
        // с указанным ключом и IV. 
        //using var AES = Aes.Create("AesManaged");
        using (RijndaelManaged rijAlg = new())
        {
            //Записываем ключ в байтах
            rijAlg.Key = Encoding.UTF8.GetBytes(key);
            //Генерируем массив вектора в байтах
            rijAlg.GenerateIV();
            iv = rijAlg.IV;

            // TransformFinalBlock - это специальная функция для преобразования последнего блока или частичного блока в потоке.
            // Возвращает новый массив, содержащий оставшиеся преобразованные байты. Возвращается новый массив, потому что количество
            // информация, возвращаемая в конце, может быть больше одного блока при добавлении заполнения.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            // Создаём поток для записи шифрования. 
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                // Записываем все данные в поток
                swEncrypt.Write(plainText);
            }
            encrypted = msEncrypt.ToArray();
        }
        // Возвращаем данные в строке
        return ByteArrayToString(iv) + ":" + ByteArrayToString(encrypted);

    }
    #endregion

    #region Декодирование данных формата IV : Данные
    static string Decrypt(string cipherText, string key)
    {
        // Проверяем аргуметы 
        if (cipherText is not { Length: > 0 })
        {
            throw new ArgumentNullException("Текст декодирования является пустым");
        }

        if (key is not { Length: > 0 })
        {
            throw new ArgumentNullException("Ключ декодирования является пустым");
        }

        //Разбиваем значение до ":"
        string[] splitchiper = cipherText.Split(':');

        //Записываем второе значение
        byte[] chiperByte = StringToByteArray(splitchiper[1]);

        // Создайте объект RijndaelManaged
        // с указанным ключом и IV. 
        using RijndaelManaged rijAlg = new();
        rijAlg.Key = Encoding.UTF8.GetBytes(key);
        rijAlg.GenerateIV();

        // Создаём дешифратор для выполнения преобразования потока.
        ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

        // Создаём поток для чтения дешифрованных данных (байтов). 
        using MemoryStream msDecrypt = new(chiperByte);
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);
        // Объявляем строку, используемую для хранения
        // расшифрованный текст.
        // Читаем дешифрованные байты из потока дешифрования
        // и помещаем их в строку.
        string? plaintext = srDecrypt.ReadToEnd();
        return plaintext;
    }
    #endregion

}
