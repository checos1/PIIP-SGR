using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTranversalesMetas;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    public class FocalizacionPoliticasTransversalesMetasPersistencia : Persistencia, IFocalizacionPoliticasTransversalesMetasPersistencia
    {
        public FocalizacionPoliticasTransversalesMetasPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.UspPostPoliticasTransversalesMetasTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTMetasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.UspPostPoliticasTransversalesMetas(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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

        public PoliticaTMetasDto ObtenerFocalizacionPoliticasTransversales(string bpin)
        {
            try
            {
                var politicasTransversalesMetas = Contexto.UspGetPoliticasTransversalesMetas_JSON(bpin).SingleOrDefault();
                if (politicasTransversalesMetas == null)
                {
                    PoliticaTMetasDto pltm = new PoliticaTMetasDto();
                    pltm.BPIN = bpin;
                    return pltm;
                }
                return JsonConvert.DeserializeObject<PoliticaTMetasDto>(politicasTransversalesMetas);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public PoliticaTMetasDto ObtenerFocalizacionPoliticasTransversalesPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoliticaTMetasDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaFocalizacionPoliticasTransversalesMetas);
        }
    }
}
