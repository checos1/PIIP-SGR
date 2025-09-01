using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Transversales;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGP.Transversales
{
    public class TransversalPersistenciaSGP : PersistenciaSGP, ITransversalPersistenciaSGP
    {
        public TransversalPersistenciaSGP(IContextoFactory contextoFactory, IContextoFactorySGR contextoFactorySGR) : base(contextoFactory, contextoFactorySGR)
        {
        }

        public EncabezadoSGPDto ObtenerEncabezadoSGP(ParametrosEncabezadoSGP parametros)
        {
            if (parametros.Tramite == null)
            {
                parametros.Tramite = "";
            }
            EncabezadoSGPDto resultado = ContextoOnlySP.Database.SqlQuery<EncabezadoSGPDto>("Proyectos.uspGetSGP_Encabezado_LeerEncabezado @idInstancia,@idFlujo,@idNivel,@idProyecto,@tramite ",
                                new SqlParameter("idInstancia", parametros.IdInstancia),
                                new SqlParameter("idFlujo", parametros.IdFlujo),
                                new SqlParameter("idNivel", parametros.IdNivel),
                                new SqlParameter("idProyecto", parametros.IdProyecto),
                                new SqlParameter("tramite", parametros.Tramite)
                                 ).FirstOrDefault();

            return resultado;
        }
    }
}
