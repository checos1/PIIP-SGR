using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Proyectos
{
    using System.Data.Entity.Core.Objects;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Comunes;
    using Newtonsoft.Json.Linq;
    using Dominio.Dto.Proyectos;
    using Interfaces;
    using Interfaces.Proyectos;
    using Modelo;
    using Newtonsoft.Json;

    public class DefinirAlcancePersistencia : Persistencia, IDefinirAlcancePersistencia
    {
        public DefinirAlcancePersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public AlcanceDto ObtenerDefinirAlcance(string bpin)
        {
            try
            {
                var DefinirAlcanceDto = Contexto.UspGetAlcance_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<AlcanceDto>(DefinirAlcanceDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostAlcanceTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public AlcanceDto ObtenerDefinirAlcancePreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<AlcanceDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewDefinirAlcance);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<AlcanceDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostAlcance(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
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
    }
}
