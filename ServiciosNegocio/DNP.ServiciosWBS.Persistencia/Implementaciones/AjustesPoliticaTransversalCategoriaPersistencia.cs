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
    public class AjustesPoliticaTransversalCategoriaPersistencia : Persistencia, IAjustesPoliticaTransversalCategoriaPersistencia
    {
        public AjustesPoliticaTransversalCategoriaPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public AjustesPoliticaTCategoriasDto ObtenerAjustesPoliticaTransversalCategoria(string bpin)
        {
            try
            {
                var ajustesPoliticaTCategoriasDto = Contexto.UspGetPoliticasTransversalesBeneficiariosCategorias_Ajustes_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<AjustesPoliticaTCategoriasDto>(ajustesPoliticaTCategoriasDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostPoliticasTransversalesBeneficiariosCategoriasTemp_Ajustes(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public AjustesPoliticaTCategoriasDto ObtenerAjustesPoliticaTransversalCategoriaPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<AjustesPoliticaTCategoriasDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewAjustesPoliticaTCategoria);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<AjustesPoliticaTCategoriasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesBeneficiariosCategorias_Ajustes(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
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
