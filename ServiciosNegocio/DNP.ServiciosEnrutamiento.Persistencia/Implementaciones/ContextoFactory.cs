using DNP.ServiciosEnrutamiento.Persistencia.Interfaces;
using DNP.ServiciosEnrutamiento.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosEnrutamiento.Persistencia.Implementaciones
{
    [ExcludeFromCodeCoverage]
    public class ContextoFactory: IContextoFactory
    {
        public MGAWebContexto CrearContexto(DbConnection conexion)
        {
            return new MGAWebContexto();
        }

        public MGAWebContexto CrearContextoConConexion(string cadenaCompleta)
        {
            return ConstructorDeConexion.CrearContextoProgramatico(cadenaCompleta);
        }
    }
}
