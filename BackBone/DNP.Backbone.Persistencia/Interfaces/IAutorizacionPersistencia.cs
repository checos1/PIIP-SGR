using DNP.Backbone.Comunes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Persistencia.Interfaces
{
    public interface IAutorizacionPersistencia
    {
        RespuestaGeneralDto GuardarDatosMongoDB(dynamic data, string idSql);
        dynamic ObtenerDatosMongoDb(string id);
    }
}
