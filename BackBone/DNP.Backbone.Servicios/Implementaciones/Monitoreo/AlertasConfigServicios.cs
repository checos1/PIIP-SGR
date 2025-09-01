namespace DNP.Backbone.Servicios.Implementaciones.Monitoreo
{
    using Comunes.Dto;
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using DNP.Backbone.Servicios.Validadores;
    using Interfaces.ServiciosNegocio;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Clase responsable de la gestión de servicio de alertas config
    /// </summary>
    public class AlertasConfigServicios : IAlertasConfigServicios
    {
        private readonly IFlujoServicios _flujoServicios;
        private readonly AlertasCondigValidator _validator;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="flujoServicios">Instancia de servicios de flujos</param>                
        public AlertasConfigServicios(IFlujoServicios flujoServicios, AlertasCondigValidator validator)
        {
            _flujoServicios = flujoServicios;
            _validator = validator;
        }

        /// <summary>
        /// Obtención de datos de alertas config.
        /// </summary>
        /// <param name="instanciaAlertasConfigDto">Contiene informacion de autorizacion y filtros </param>
        /// <returns>consulta de datos trámite.</returns>
        public Task<List<AlertasConfigDto>> ObtenerAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            return _flujoServicios.ObtenerAlertasConfig(alertasConfigFiltroDto);
        }

        /// <summary>
        /// Creación de datos de alertas config.
        /// </summary>
        /// <param name="alertasConfigFiltroDto">Contiene informacion de autorizacion y filtros </param>
        /// <returns>datos de alertas config.</returns>
        public Task<AlertasConfigDto> CrearActualizarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            var result = _validator.Validate(alertasConfigFiltroDto.AlertasConfigDto);
            if (!result.IsValid)
                throw new BackboneException(result.IsValid, result.Errors.Select(x => x.ErrorMessage));

            return _flujoServicios.CrearActualizarAlertasConfig(alertasConfigFiltroDto);
        }

        /// <summary>
        /// Eliminación de datos de alertas config.
        /// </summary>
        /// <param name="alertasConfigFiltroDto">Contiene informacion de autorizacion y filtros </param>
        /// <returns>datos de alertas config.</returns>
        public Task<AlertasConfigDto> EliminarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            return _flujoServicios.EliminarAlertasConfig(alertasConfigFiltroDto);
        }

    }
}
