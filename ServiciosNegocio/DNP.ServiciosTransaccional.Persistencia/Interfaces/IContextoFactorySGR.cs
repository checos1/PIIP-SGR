using DNP.ServiciosTransaccional.Persistencia.ModeloSGR;
using System.Data.Common;

namespace DNP.ServiciosTransaccional.Persistencia.Interfaces
{
   public interface IContextoFactorySGR
    {
       MGAWebContextoSGR CrearContextoSGR(DbConnection conexion);
       MGAWebContextoSGR CrearContextoConConexionSGR(string cadenaCompleta);
   }
}
