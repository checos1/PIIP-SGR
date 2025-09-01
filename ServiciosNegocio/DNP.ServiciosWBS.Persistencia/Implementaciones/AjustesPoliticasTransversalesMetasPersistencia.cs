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
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using Newtonsoft.Json;

    public class AjustesPoliticasTransversalesMetasPersistencia : Persistencia, IAjustesPoliticasTransversalesMetasPersistencia
    {
        public AjustesPoliticasTransversalesMetasPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetas(string bpin)
        {
            try
            {
                var politicasTransversalesMetas = Contexto.UspGetPoliticasTransversalesMetas_Ajustar_JSON(bpin).SingleOrDefault();
                if (politicasTransversalesMetas == null)
                {
                    AjustesPoliticaTMetasDto pltm = new AjustesPoliticaTMetasDto();
                    pltm.BPIN = bpin;
                    return pltm;
                }
                return JsonConvert.DeserializeObject<AjustesPoliticaTMetasDto>(politicasTransversalesMetas);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.UspPostPoliticasTransversalesMetasTemp_Ajustar(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetasPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<AjustesPoliticaTMetasDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaPreviewAjustesPoliticaTMetas);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<AjustesPoliticaTMetasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.UspPostPoliticasTransversalesMetas_Ajustar(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

    }
}
