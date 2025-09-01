using DNP.ServiciosTransaccional.Persistencia.Modelo;
using System.Data.Common;

namespace DNP.ServiciosTransaccional.Persistencia.Interfaces
{
   public interface IContextoFactory
   {
        MGAWebContextoTransaccional CrearContexto(DbConnection conexion);
        MGAWebContextoTransaccional CrearContextoConConexion(string cadenaCompleta);
   }
}
