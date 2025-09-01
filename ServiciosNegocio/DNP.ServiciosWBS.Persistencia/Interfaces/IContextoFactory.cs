namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using System.Data.Common;
    using Modelo;

    public interface IContextoFactory
    {
        MGAWebContexto CrearContexto(DbConnection conexion);
        MGAWebContexto CrearContextoConConexion(string cadenaCompleta);
    }
}