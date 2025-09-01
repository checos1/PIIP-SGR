using Newtonsoft.Json;
using System;

namespace DNP.Backbone.Comunes.Extensiones
{
    public static class ObjetoExtensiones
    {
        public static string Serialize(this object value)
            => JsonConvert.SerializeObject(value);

        public static bool TrySerialize<TResult>(this string value, out string result)
        {
            try
            {
                result = JsonConvert.SerializeObject(value);
                return true;
            }
            catch (Exception ex)
            {
                result = default;
                return false;
            }
        }
    }
}
