using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.DelegarViabilidad;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.UI.WebControls;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGR.DelegarViabilidad
{
    public class EntidadEjecutorPersistencia : PersistenciaSGR, IEntidadEjecutorPersistencia
    {
        #region Constructor

        public EntidadEjecutorPersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {
        }

        /// <summary>
        /// Permite guardar el Ejecutor Asociado
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <param name="ejecutorId"></param>
        /// <param name="usuario"></param>
        /// <param name="tipoEjecutorId"></param>
        /// <returns></returns>
        public bool CrearEjecutorAsociado(int proyectoId, int ejecutorId, string usuario, int tipoEjecutorId)
        {
            var EjecutorProcesado = Contexto.uspPostEjecutoresAsociados(proyectoId, ejecutorId, usuario, tipoEjecutorId);

            return EjecutorProcesado > 0;
        }

        /// <summary>
        /// Permite eliminar un ejecutor Asociado
        /// </summary>
        /// <param name="EjecutorAsociadoId"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public SeccionesEjecutorEntidad EliminarEjecutorAsociado(int EjecutorAsociadoId, string usuario)
        {
            var resultado = new SeccionesEjecutorEntidad();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
                    Contexto.uspPostEliminarEjecutorAsociado(EjecutorAsociadoId, usuario, errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = "Capitulo modificado Eliminado Exitosamente!";
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado.Exito = false;
                        resultado.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Obtener el Listado de Ejecutores por filtro
        /// </summary>
        /// <param name="nit"></param>
        /// <param name="tipoEntidadId"></param>
        /// <param name="entidadId"></param>
        /// <returns></returns>
        public List<EjecutorEntidadDto> ObtenerListadoEjecutores(string nit, int? tipoEntidadId, int? entidadId)
        {
            var resultSp = Contexto.uspGetTipoEjecutor(nit, tipoEntidadId, entidadId).ToList();

            var ejecutorList = new List<EjecutorEntidadDto>();

            ejecutorList = resultSp.Select(est => new EjecutorEntidadDto()
            {
                Entidad = est.Entidad,
                Id = est.Id,
                Nit = est.Nit,
                TipoEntidad = est.TipoEntidad
            }).ToList();

            return ejecutorList;
        }

        /// <summary>
        /// Obtener listado de ejecutores asociados
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <returns></returns>
        public List<EjecutorEntidadAsociado> ObtenerListadoEjecutoresAsociados(int proyectoId)
        {
            var resultSp = Contexto.uspGetEjecutoresAsociados(proyectoId).ToList();

            var ejecutorList = new List<EjecutorEntidadAsociado>();

            ejecutorList = resultSp.Select(est => new EjecutorEntidadAsociado()
            {
                EjecutorId = est.EjecutorId,
                Id = est.id,
                NitEjecutor = est.NitEjecutor,
                NombreEntidad = est.nombreEntidad,
                TipoEntidad = est.TipoEntidad
            }).ToList();

            return ejecutorList;
        }

        #endregion
    }
}


