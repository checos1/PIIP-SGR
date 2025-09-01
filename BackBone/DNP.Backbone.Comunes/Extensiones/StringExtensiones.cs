using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace DNP.Backbone.Comunes.Extensiones
{
    public static class StringExtensiones
    {
        public static TResult Deserialize<TResult>(this string value)
            => JsonConvert.DeserializeObject<TResult>(value);

        public static bool TryDeserialize<TResult>(this string value, out TResult result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<TResult>(value);
                return true;
            }
            catch (Exception ex)
            {
                result = default;
                return false;
            }
        }

        public static string EncodeToBase64(this string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeFromBase64(this string encondeText)
        {
            var bytesDecode = Convert.FromBase64String(encondeText);
            return Encoding.UTF8.GetString(bytesDecode);
        }
    }
}
