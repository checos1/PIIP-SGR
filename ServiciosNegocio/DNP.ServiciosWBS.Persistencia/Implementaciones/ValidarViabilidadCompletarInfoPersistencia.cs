namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using AutoMapper;
    using Interfaces;
    using Modelo;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;

    public class ValidarViabilidadCompletarInfoPersistencia : Persistencia, IValidarViabilidadCompletarInfoPersistencia
    {
        public ValidarViabilidadCompletarInfoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostProyectoCompletarInformacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
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
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

        }

        public ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfo(ParametrosConsultaDto parametrosConsultaDto)
        {
            if (string.IsNullOrEmpty(parametrosConsultaDto.Bpin)) return null;
            var consultaDesdeBd = Contexto.uspGetProyectoCompletarInformacion(parametrosConsultaDto.Bpin, parametrosConsultaDto.Usuario, parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
            return MapearAValidarViabilidadCompletarInfoDto(consultaDesdeBd.ToList());
        }

        private ValidarViabilidadCompletarInfoDto MapearAValidarViabilidadCompletarInfoDto(IEnumerable<uspGetProyectoCompletarInformacion_Result> consultaDesdeBd)
        {
            var validarViabilidadCompletarInfo = new ValidarViabilidadCompletarInfoDto();
            validarViabilidadCompletarInfo.Tematicas = new List<TematicaDto>();
            consultaDesdeBd.GroupBy(o1 => new
            {
                o1.Bpin,
                o1.Mensaje,
               o1.Tematica
            }).ToList().ForEach(w1 => {
                   var auxmensajes = new List<MensajeErrorDto>();
                   consultaDesdeBd.Where(v => v.Tematica == w1.Key.Tematica).GroupBy(tematica =>
                  new
                  {
                      tematica.Tematica,
                      tematica.MensajeError
                      
                  }).ToList().ForEach(t1 => {
                     
                      auxmensajes.Add(new MensajeErrorDto() { MensajeError = t1.Key.MensajeError });
                   });
                validarViabilidadCompletarInfo.Tematicas.Add(new TematicaDto()
                {
                    Tematica = w1.Key.Tematica,
                    MensajesError = auxmensajes.ToList()
                });
                validarViabilidadCompletarInfo.Bpin = w1.Key.Bpin;
                validarViabilidadCompletarInfo.Mensaje = w1.Key.Mensaje;
            });
         return validarViabilidadCompletarInfo;
        }



        public ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfoPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<ValidarViabilidadCompletarInfoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaValidacionViabilidadCompletarInfo);
        }

        #region Metodos utilitarios
        private ValidarViabilidadCompletarInfoDto MapearEntidad(uspGetProyectoCompletarInformacion_Result entidad)
        {
            return Mapper.Map<ValidarViabilidadCompletarInfoDto>(entidad);
        }
        private static void ConfigurarMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<uspGetProyectoCompletarInformacion_Result, ValidarViabilidadCompletarInfoDto>());
        }
        #endregion
    }
}
