namespace DNP.Backbone.Servicios.Implementaciones.Monitoreo
{
    using Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using Interfaces.ServiciosNegocio;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Clase responsable de la gestión de servicio de alertas config
    /// </summary>
    public class MapColumnasServicios : IMapColumnasServicios
    {
        private readonly IFlujoServicios _flujoServicios;
        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="flujoServicios">Instancia de servicios de flujos</param>                
        public MapColumnasServicios(IFlujoServicios flujoServicios)
        {
            _flujoServicios = flujoServicios;
        }

        /// <summary>
        /// Obtención de datos de map columnas.
        /// </summary>
        /// <param name="mapColumnasFiltroDto">Contiene informacion de autorizacion y filtros </param>
        /// <returns>consulta de datos de map columnas.</returns>
        public Task<List<MapColumnasDto>> ObtenerMapColumnas(MapColumnasFiltroDto mapColumnasFiltroDto)
        {
            return _flujoServicios.ObtenerMapColumnas(mapColumnasFiltroDto);
        }

    }
}
