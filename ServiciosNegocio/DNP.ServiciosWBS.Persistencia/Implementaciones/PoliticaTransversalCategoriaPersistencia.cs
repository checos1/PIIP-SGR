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
    public class PoliticaTransversalCategoriaPersistencia : Persistencia, IPoliticaTransversalCategoriaPersistencia
    {
        public PoliticaTransversalCategoriaPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoria(string bpin)
        {
            try
            {
                var cuantificacionLocalizacionDto = Contexto.UspGetPoliticasTransversalesBeneficiariosCategorias_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<PoliticaTCategoriasDto>(cuantificacionLocalizacionDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostPoliticasTransversalesBeneficiariosCategoriasTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoriaPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoliticaTCategoriasDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewPoliticaTCategoria);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTCategoriasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesBeneficiariosCategorias(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
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
