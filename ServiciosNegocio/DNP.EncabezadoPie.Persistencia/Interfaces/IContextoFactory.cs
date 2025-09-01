using DNP.EncabezadoPie.Persistencia.Modelo;
using System.Data.Common;

namespace DNP.EncabezadoPie.Persistencia.Interfaces
{
    public interface IContextoFactory
    {
        MGAWebContextoEncabezado CrearContexto(DbConnection conexion);
        MGAWebContextoEncabezado CrearContextoConConexion(string cadenaCompleta);
    }
}
