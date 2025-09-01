namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using Interfaces;
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Tramites;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using Newtonsoft.Json;
    public class CartaConceptoDatosDespediaPersistencia : Persistencia, ICartaConceptoDatosDespediaPersistencia
    {
        public CartaConceptoDatosDespediaPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public DatosConceptoDespedidaDto ObtenerCartaConceptoDatosDespedida(int TramiteId, int plantillaCartaSeccionId)
        {
            try
            {
                var ConceptoDatosDespedidaDto = Contexto.uspGetDatosCartaConceptoDespedida_JSON(TramiteId, plantillaCartaSeccionId).SingleOrDefault();
                return JsonConvert.DeserializeObject<DatosConceptoDespedidaDto>(ConceptoDatosDespedidaDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public DatosConceptoDespedidaDto ObtenerCartaConceptoDatosDespedidaPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<DatosConceptoDespedidaDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaDatosCartaConceptoDespedida);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<DatosConceptoDespedidaDto> parametrosGuardar,  string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                   
                  Contexto.uspPostActualizaCartaDatosDespedida(parametrosGuardar.Contenido.CartaId, parametrosGuardar.Contenido.CartaSeccionId, parametrosGuardar.Contenido.TramiteId, parametrosGuardar.Contenido.TipoTramite, JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario,errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

        }
    }
}
