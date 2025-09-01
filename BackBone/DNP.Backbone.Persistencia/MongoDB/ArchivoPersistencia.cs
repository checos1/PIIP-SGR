using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Persistencia.MongoDB
{
    public class ArchivoPersistencia
    {
        public bool MongoDBInsertOne(ArchivodeCargue archivo)
        {
            try
            {
                MongoHelper.ConnectionToMongoServices();
                var collection = MongoHelper.database.GetCollection<BsonDocument>("ArchivosCargue");
                BsonDocument bsonObject = archivo.ToBsonDocument();
                collection.InsertOne(bsonObject);
                
                return true;
            }
            catch (MongoException mx)
            {

                throw new Exception(mx.Message);
            }

        }
        //obter el arquivo pelo id do SQL
        public BsonDocument MongoDBFindByFilter(string idSQL)
        {
            try
            {
                MongoHelper.ConnectionToMongoServices();
                var collection = MongoHelper.database.GetCollection<BsonDocument>("ArchivosCargue");
                var filter = Builders<BsonDocument>.Filter.Eq("IDSql", idSQL);

                return collection.Find(filter).FirstOrDefault();
            }
            catch (MongoException mx)
            {
                throw new Comunes.Excepciones.BackboneException(mx.Message);
            }
        }

        //obter o listado completo
        public object MongoRetriveData()
        {
            try
            {
                MongoHelper.ConnectionToMongoServices();
                var collection = MongoHelper.database.GetCollection<BsonDocument>("ArchivosCargue");

                var firstDocument = collection.Find(new BsonDocument()).FirstOrDefault();
                Console.WriteLine(firstDocument.ToString());



                return firstDocument;
            }
            catch (MongoException mx)
            {

                throw new Comunes.Excepciones.BackboneException(mx.Message);
            }

        }

    }
}
