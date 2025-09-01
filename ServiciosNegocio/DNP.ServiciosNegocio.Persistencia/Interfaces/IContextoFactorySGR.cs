using System.Data.Common;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces
{
   public interface IContextoFactorySGR
   {
       MGAWebContextoSGR CrearContextoSGR(DbConnection conexion);
       MGAWebContextoSGR CrearContextoConConexionSGR(string cadenaCompleta);
   }
}
