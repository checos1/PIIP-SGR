using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace DNP.ServiciosNegocio.Comunes.Utilidades
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class JsonUtilidades
    {
        public static string ACadenaJson<T>(T objeto)
        {
            return JsonConvert.SerializeObject(objeto);
        }

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

        public static object Convertir(string ruta)
        {
            using (StreamReader file = File.OpenText(ruta))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                IDictionary<string, object> dictionary = serializer.Deserialize<IDictionary<string, object>>(file.ReadToEnd());
                return dictionary.Expando();
            }
        }


        public static ExpandoObject Expando(this IDictionary<string, object> dictionary)
        {
            ExpandoObject expandoObject = new ExpandoObject();
            IDictionary<string, object> objects = expandoObject;

            foreach (var item in dictionary)
            {
                bool processed = false;

                if (item.Value is IDictionary<string, object>)
                {
                    objects.Add(item.Key, Expando((IDictionary<string, object>)item.Value));
                    processed = true;
                }
                else if (item.Value is ICollection)
                {
                    List<object> itemList = new List<object>();

                    foreach (var item2 in (ICollection)item.Value)

                        if (item2 is IDictionary<string, object>)
                            itemList.Add(Expando((IDictionary<string, object>)item2));
                        else
                            itemList.Add(Expando(new Dictionary<string, object> { { "Unknown", item2 } }));

                    if (itemList.Count > 0)
                    {
                        objects.Add(item.Key, itemList);
                        processed = true;
                    }
                }

                if (!processed)
                    objects.Add(item);
            }

            return expandoObject;
        }

        public static T SerializarJsonObjeto<T>(object p)
        {
            throw new System.NotImplementedException();
        }
    }
}
