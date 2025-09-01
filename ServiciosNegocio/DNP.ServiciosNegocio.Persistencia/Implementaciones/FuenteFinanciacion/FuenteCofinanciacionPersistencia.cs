using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Modelo;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.FuenteFinanciacion
{
    using System.Data.Entity.Core.Objects;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Comunes;
    using Newtonsoft.Json;

    public class FuenteCofinanciacionPersistencia : Persistencia, IFuenteCofinanciacionPersistencia
    {
        public FuenteCofinanciacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();
        }

        public FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyecto(string bpin)
        {
            try
            {
                var listadoFuentesFinanciacion = Contexto.uspGetFuentesFinanciacion_Cofinanciacion_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<FuenteCofinanciacionProyectoDto>(listadoFuentesFinanciacion);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostFuenteCofinanciacionTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyectoPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<FuenteCofinanciacionProyectoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                       @RutasPreviewRecursos.RutaPreviewFuenteCofinanciacion);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<FuenteCofinanciacionProyectoDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFuenteCofinanciacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
                                                         usuario,
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
    }
}
