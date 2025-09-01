using AutoMapper;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;


namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGR.Viabilidad 
{
    public class AcuerdoPersistencia : PersistenciaSGR, IAcuerdoPersistencia
    {
        #region Constructor

        public AcuerdoPersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {            
            ConfigurarMapper();
        }


        #endregion

        #region "Métodos"

        /// <summary>
        /// Leer el acuerdo, sector y clasificadores de un proyecto
        /// </summary>
        public string SGR_Acuerdo_LeerProyecto(int proyectoId, System.Guid nivelId)
        {
            string Json = Contexto.uspGetSGR_Acuerdo_LeerProyecto(proyectoId, nivelId).SingleOrDefault();
            return Json;
        }

        public ResultadoProcedimientoDto SGR_Acuerdo_GuardarProyecto(string json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostSGR_Acuerdo_GuardarProyecto(json, usuario, errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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

        private static void ConfigurarMapper()
        {
            Mapper.Reset();
        }

        #endregion
    }
}
