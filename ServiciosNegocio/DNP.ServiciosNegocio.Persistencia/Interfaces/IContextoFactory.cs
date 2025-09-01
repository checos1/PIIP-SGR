using System.Data.Common;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Persistencia.Modelo_OnlySP;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces
{
    public interface IContextoFactory
    {
        ModelOnlySPEntities CrearContextoOnlySP(DbConnection conexion);
        ModelOnlySPEntities CrearContextoConConexionOnlySP(string cadenaCompleta);
        MGAWebContexto CrearContexto(DbConnection conexion);
        MGAWebContexto CrearContextoConConexion(string cadenaCompleta);


    }
}
