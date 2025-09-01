using AutoMapper;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.IndicadoresPolitica;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.IndicadoresPolitica
{
    public class CategoriaProductosPoliticaPersistencia : Persistencia, ICategoriaProductosPoliticaPersistencia
    {
        #region Constructor

        public CategoriaProductosPoliticaPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();
        }

        #endregion

        #region Obtener Datos Categoria Productos Politica

        /// <summary>
        /// Obtener Datos Categoria Productos Politica
        /// </summary>
        /// <param name="Bpin"></param>
        /// <returns></returns>
        public string ObtenerDatosCategoriaProductosPolitica(string Bpin, int fuenteId, int politicaId)
        {
            return Contexto.uspGetPoliticasTransversalesCategorias_JSON(Bpin, fuenteId, politicaId).FirstOrDefault();
        }

        #endregion

        #region Guardar Datos Solicitud Recursos

        /// <summary>
        /// Guardar DatosSolicitud Recursos
        /// </summary>
        /// <param name="Bpin"></param>
        /// <returns></returns>
        public string GuardarDatosSolicitudRecursos(ParametrosGuardarDto<CategoriaProductoPoliticaDto> categoriaProductoPoliticaDto, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesCategorias(JsonUtilidades.ACadenaJson(categoriaProductoPoliticaDto.Contenido), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == categoriaProductoPoliticaDto.InstanciaId && at.AccionId == categoriaProductoPoliticaDto.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "ok";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        //throw new ServiciosNegocioException(mensajeError);
                        return mensajeError;
                    }
                }
                catch (ServiciosNegocioException ex)
                {
                    dbContextTransaction.Rollback();
                    return ex.Message;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return ex.Message;
                }
            }
        }

        #endregion
    }
}
