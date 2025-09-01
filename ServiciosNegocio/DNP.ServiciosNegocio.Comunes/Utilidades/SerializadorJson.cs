using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IO;


namespace DNP.ServiciosNegocio.Comunes.Utilidades
{
    [ExcludeFromCodeCoverage]
    public static class SerializadorJson
    {
        public static T SerializarJsonObjeto<T>(string ruta)
        {
            T objetoSerealizado;

            using (StreamReader file = File.OpenText(ruta))
            {
                JsonSerializer serializer = new JsonSerializer();
                objetoSerealizado = (T)serializer.Deserialize(file, typeof(T));
            }

            return objetoSerealizado;
        }
    }
}
