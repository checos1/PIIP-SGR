using DNP.ServiciosEnrutamiento.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosEnrutamiento.Persistencia.Interfaces
{
    public interface IContextoFactory
    {
        MGAWebContexto CrearContexto(DbConnection conexion);
        MGAWebContexto CrearContextoConConexion(string cadenaCompleta);
    }
}
