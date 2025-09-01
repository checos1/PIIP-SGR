using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Modelo;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using Newtonsoft.Json;
    using DNP.ServiciosWBS.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;

    public class LocalizacionPersistencia : Persistencia, ILocalizacionPersistencia
    {
        private readonly ISeccionCapituloPersistencia _seccionCapituloPersistencia;
        public LocalizacionPersistencia(IContextoFactory contextoFactory, ISeccionCapituloPersistencia seccionCapituloPersistencia) : base(contextoFactory)
        {
            _seccionCapituloPersistencia = seccionCapituloPersistencia;
        }

        public LocalizacionProyectoDto Obtenerlocalizacion(string bpin)
        {
            try
            {
                var LocalizacionDto = Contexto.UspGetLocalizacion_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<LocalizacionProyectoDto>(LocalizacionDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostLocalizacionTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public LocalizacionProyectoDto ObtenerlocalizacionPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<LocalizacionProyectoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewlocalizacion);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<LocalizacionProyectoDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostLocalizacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
                                                usuario,
                                                parametrosGuardar.InstanciaId,
                                                parametrosGuardar.AccionId,
                                                parametrosGuardar.FormularioId,
                                                errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
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

        public ResultadoProcedimientoDto GuardarLocalizacion(LocalizacionProyectoAjusteDto localizacionProyecto, string usuario)
        {
            //2022-05-26 Seccion reemplaza el procedimiento almacenado de uspPostLocalizacion a uspPostLocalizacion_Ajustes

            var respuesta = new ResultadoProcedimientoDto();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostLocalizacion_Ajustes(JsonUtilidades.ACadenaJson(localizacionProyecto), usuario, resultado);
                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();

                        var capituloModificado = new CapituloModificado()
                        {
                            InstanciaId = localizacionProyecto.InstanciaId,
                            Justificacion = string.IsNullOrEmpty(localizacionProyecto.Justificacion) ? null : localizacionProyecto.Justificacion,
                            ProyectoId = localizacionProyecto.ProyectoId,
                            SeccionCapituloId = localizacionProyecto.SeccionCapituloId,
                            Usuario = usuario,
                            AplicaJustificacion = 1
                        };
                        _seccionCapituloPersistencia.GuardarJustificacionCambios(capituloModificado);
                        
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    return respuesta;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
    }
}
