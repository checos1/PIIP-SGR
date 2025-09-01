using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Proyectos
{
    public class AjusteIncluirPoliticasPersistencia : Persistencia, IAjusteIncluirPoliticasPersistencia
    {
        public AjusteIncluirPoliticasPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
           
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostAgregarPoliticas_AjustarTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostAgregarPoliticas_Ajustar(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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

        public IncluirPoliticasDto ObtenerAjusteIncluirPoliticas(string bpin)
        {
            
            try
            {
                var politicasTransversalesMetas = Contexto.uspGetAgregarPoliticas_Ajustar_JSON(bpin).SingleOrDefault();
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

        public IncluirPoliticasDto ObtenerAjusteIncluirPoliticasPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<IncluirPoliticasDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaAjusteIncluirPoliticas);
        }
    }
}
