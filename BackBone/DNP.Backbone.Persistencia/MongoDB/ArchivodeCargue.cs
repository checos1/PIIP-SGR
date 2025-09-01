using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DNP.Backbone.Persistencia.MongoDB
{
    public class ArchivodeCargue
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }


        public string IDSql;
        public string TipoDeArchivo { get; set; }

        public DateTime Fecha { get; set; }


        public int IdTIpoArchivo { get; set; }

        public int IdEntidad { get; set; }

        public object InfoArchivo { get; set; }

    }
}
