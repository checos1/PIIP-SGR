using MongoDB.Driver;
using System;

namespace DNP.Backbone.Persistencia.MongoDB
{
    public class MongoHelper
    {
        public static IMongoClient client { get; set; }
        public static IMongoDatabase database { get; set; }
        public static string MongoConnection = "mongodb+srv://ntconsult:ntconsult@cluster0.sgjrh.azure.mongodb.net/Archivos?retryWrites=true&w=majority";
        public static string MongoDataBase = "ArchivosCargue";

        internal static void ConnectionToMongoServices()
        {
            try
            {
                client = new MongoClient(MongoConnection);
                database = client.GetDatabase(MongoDataBase);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
