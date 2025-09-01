using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.CostosActividades;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursos;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    public class CostosActividadesPersistencia : Persistencia, ICostosActividadesPersistencia
    {
        public CostosActividadesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
                
        }

        public CostosActividadesDto ObtenerCostosActividades(string bpin)
        {
            try
            {
                var metasrecursos = Contexto.UspGetCadenaValor_Ajustes_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<CostosActividadesDto>(metasrecursos);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
           // Contexto.UspPostRegionalizacionMetasRecursosTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<CostosActividadesDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCadenaValor_Ajustes(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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
                catch (ServiciosNegocioException e)
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

        public CostosActividadesDto ObtenerCostosActividadesPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<CostosActividadesDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaCostosActividades);
        }

        public void GuardarAjusteCostoActividades(ParametrosGuardarDto<ProductoAjusteDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    //Contexto.uspPostCadenaValor_AjusteCostoActividades(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

                    //if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    //{
                    //    var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                    //    if (temporal != null)
                    //        Contexto.AlmacenamientoTemporal.Remove(temporal);

                    //    Contexto.SaveChanges();
                    //    dbContextTransaction.Commit();
                    //    return;
                    //}
                    //else
                    //{
                    //    var mensajeError = Convert.ToString(resultado.Value);
                    //    throw new ServiciosNegocioException(mensajeError);
                    //}
                }
                catch (ServiciosNegocioException e)
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
