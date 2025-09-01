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

    public class IncluirPoliticasPersistencia : Persistencia, IIncluirPoliticasPersistencia
    {
        public IncluirPoliticasPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public IncluirPoliticasDto ObtenerIncluirPoliticas(string bpin)
        {
            try
            {
                var politicasTransversalesMetas = Contexto.uspGetAgregarPoliticasTransversalesJerarquia_JSON(bpin).SingleOrDefault();
                if (politicasTransversalesMetas == null)
                {
                    IncluirPoliticasDto pltm = new IncluirPoliticasDto();
                    pltm.BPIN = bpin;
                    return pltm;
                }
                return JsonConvert.DeserializeObject<IncluirPoliticasDto>(politicasTransversalesMetas);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostAgregarPoliticasTransversalesJerarquiaTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public IncluirPoliticasDto ObtenerIncluirPoliticasPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<IncluirPoliticasDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaIncluirPoliticasSN);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostAgregarPoliticasTransversalesJerarquia(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
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
