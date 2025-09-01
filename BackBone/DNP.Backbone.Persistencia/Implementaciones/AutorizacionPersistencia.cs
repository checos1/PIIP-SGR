namespace DNP.Backbone.Persistencia.Implementaciones
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Persistencia.MongoDB;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.IO;
    using Interfaces;
    using Newtonsoft.Json.Linq;
    using System;

    public class AutorizacionPersistencia : IAutorizacionPersistencia
    {
       
        public AutorizacionPersistencia()
        {

        }

        public RespuestaGeneralDto GuardarDatosMongoDB(dynamic data, string idSql)
        {
            ArchivoPersistencia dbAccess = new ArchivoPersistencia();

            ArchivodeCargue archivo = new ArchivodeCargue()
            {
                Id = ObjectId.GenerateNewId(),
                IDSql = idSql,
                Fecha = DateTime.Now,
                IdEntidad = 1,
                IdTIpoArchivo = 1,
                InfoArchivo = data
            };

            try
            {
                var respuesta = dbAccess.MongoDBInsertOne(archivo);
                dbAccess.MongoRetriveData();
                return new RespuestaGeneralDto() { Exito = respuesta };
            }
            catch(Exception ex)
            {
                return new RespuestaGeneralDto() { Exito = false, Mensaje = ex.Message };
            }
        }

        public dynamic ObtenerDatosMongoDb(string idSql)
        {
            ArchivoPersistencia dbAccess = new ArchivoPersistencia();

            var respuesta =  dbAccess.MongoDBFindByFilter(idSql);
            var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            JObject json = JObject.Parse(respuesta.ToJson<BsonDocument>(jsonWriterSettings));
            // var teste = JsonConvert.DeserializeObject<ArchivodeCargue>(respuesta);

            return json;
        }




    }
}
