namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using Interfaces;
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Productos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using Newtonsoft.Json;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosWBS.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;

    public class DesagregarRegionalizacionPersistencia : Persistencia, IDesagregarRegionalizacionPersistencia
    {

        private readonly ISeccionCapituloPersistencia _seccionCapituloPersistencia;

        public DesagregarRegionalizacionPersistencia(IContextoFactory contextoFactory, ISeccionCapituloPersistencia seccionCapituloPersistencia) : base(contextoFactory)
        {
            _seccionCapituloPersistencia = seccionCapituloPersistencia;
        }

        public DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacion(string bpin)
        {
            try
            {
                var UbicacionesDto = Contexto.UspGetDesagregarRegionalizacion_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<DesagregarRegionalizacionDto>(UbicacionesDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacionPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<DesagregarRegionalizacionDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewDesagregarRegionalizacion);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<DesagregarRegionalizacionDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                  
                    Contexto.UspPostDesagregarRegionalizacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),usuario,errorValidacionNegocio);

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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

        }
    }
}
