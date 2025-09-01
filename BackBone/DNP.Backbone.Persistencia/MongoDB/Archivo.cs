using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DNP.Backbone.Persistencia.MongoDB
{
    public class Archivo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public String Collection { get; set; }

        public DateTime Fecha { get; set; }

        [BsonExtraElements]
        public Dictionary<string, object> Metadatos { get; set; }

    }

}
