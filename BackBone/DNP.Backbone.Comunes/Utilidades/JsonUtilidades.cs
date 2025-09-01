namespace DNP.Backbone.Comunes.Utilidades
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Newtonsoft.Json;

    [ExcludeFromCodeCoverage]
    public static class JsonUtilidades
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

        public static string ACadenaJson<T>(T objeto)
        {
            return JsonConvert.SerializeObject(objeto);
        }



    }
}
